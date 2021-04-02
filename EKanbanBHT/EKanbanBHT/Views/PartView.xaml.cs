using EKanbanBHT.Models;
using EKanbanBHT.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EKanbanBHT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PartView : ContentPage
    {
        public static KanbanItemRepository kanbanItemRepo { get; private set; }
        PartViewModel partVM { get; set; }
        List<KanbanScan> scanList { get; set; }
        KanbanScan kanbanScan { get; set; }
        public PartView(PartViewModel viewModel)
        {
            InitializeComponent();

            kanbanItemRepo = new KanbanItemRepository();
            scanList = new List<KanbanScan>();
            partVM = viewModel;
            partVM.Navigation = Navigation;
            BindingContext = partVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            QRMaterialText.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            var view = Locator.Resolve<PickingView>();
            var viewModel = view.BindingContext as PickingViewModel;
            viewModel.ScanText = "";

            Navigation.PushAsync(view);
            return true;
        }

        //private async void SubmitButton_Clicked(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(QRMaterialText.Text))
        //        await DisplayAlert("Warning", "You have to fill Scan Material.", "OK");
        //    else if (QRMaterialText.Text.Length != 223 && QRMaterialText.Text.Length != 115)
        //        await DisplayAlert("Warning", "Wrong format Scan Result (length is not 223 or 115)", "OK");
        //    else
        //    {
        //        string partNo = "";
        //        if (QRMaterialText.Text.Length == 223) partNo = QRMaterialText.Text.Substring(96, 15).Trim();
        //        if (QRMaterialText.Text.Length == 115) partNo = QRMaterialText.Text.Substring(8, 16).Trim();
        //        List<KanbanItem> items = partVM.KanbanItems.Where(a => a.PartNo == partNo).ToList();
        //        if (items.Count == 0 || items==null)
        //        {
        //            await DisplayAlert("Warning", "There's no part no " + partNo + " in the item list", "OK");
        //        }
        //        else
        //        {
        //            bool canAdd=false;
        //            int i = 0;
        //            foreach (KanbanItem item in items)
        //            {
        //                if (item.Balance > 0)
        //                {
        //                    item.ScanQty++;
        //                    item.Balance--;
        //                    canAdd = true;
        //                    i++;
        //                    break;
        //                }
        //            }
        //            if(!canAdd) 
        //                await DisplayAlert("Warning", "Can't add part " + partNo + ", the required qty has been fulfilled.", "OK");

        //        }
        //    }
        //}

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            List<KanbanItem> parts = partVM.KanbanItems.Where(a => a.Balance > 0).ToList();
            if (parts.Count > 0)
            {
                await DisplayAlert("Warning", "There's still parts not fulfilled.", "OK");
            }
            else
            {
                partVM.KanbanHeader.PickEnd = DateTime.Now;
                partVM.KanbanHeader.PickerName = Preferences.Get("user", "");
                kanbanItemRepo.KanbanSave(partVM.KanbanHeader.KanbanReqId, partVM.KanbanHeader.PickStart.Value);
                kanbanItemRepo.KanbanScanSave(scanList);
                await DisplayAlert("Save Success", "Kanban Picking has been save.", "OK");

                //redirect to picking view
                //var view = Locator.Resolve<PickingView>();
                //var viewModel = view.BindingContext as PickingViewModel;
                ////viewModel.EmpNo = this.EmpNo;
                //await Navigation.PushAsync(view);
                await Navigation.PopAsync();
            }
        }

        private void QRMaterialText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string qrCode = e.NewTextValue;
            if (qrCode.Length == 115 || qrCode.Length == 223)
            {
                if (ValidateQRCode(qrCode))
                {
                    int i = 0;
                    foreach(KanbanItem item in partVM.KanbanItems.ToArray())
                    {
                        if(item.PartNo== kanbanScan.PartNo && item.Balance > 0)
                        {
                            //update kanban item
                            partVM.KanbanItems[i].Balance--;
                            partVM.KanbanItems[i].ScanQty++;
                            KanbanList.ItemsSource = null;
                            KanbanList.ItemsSource = partVM.KanbanItems;

                            //update kanban scan
                            kanbanScan.ReqItemId = item.ReqItemId;
                            kanbanScan.KanbanReqId = item.KanbanReqId;
                            if (char.IsNumber(kanbanScan.PartNo[0])) kanbanScan.TagSeqNo = i + 1;
                            kanbanScan.DeviceId = Preferences.Get("device", "");
                            kanbanScan.EmpNo = Preferences.Get("user", "");
                            scanList.Add(kanbanScan);

                            break;
                        }
                        i++;
                    }
                }
                QRMaterialText.Text = "";
            }
        }

        private bool ValidateQRCode(string qrCode)
        {
            bool valid = false;
            string StatusMessage = "";
            //double lotSize = 0;
            //int? tagSeqNo;
            //string supplierCode = "";
            partVM.PartNo = "";
            kanbanScan = new KanbanScan();
            kanbanScan.ScanDateTime = DateTime.Now;
            if (qrCode.Length == 223)
            {
                kanbanScan.PartNo = qrCode.Substring(96, 15).Trim();
                try
                {
                    kanbanScan.QtyUnit = Convert.ToDouble(qrCode.Substring(111, 7));
                }
                catch
                {
                    StatusMessage += "Lot Size format is wrong.\n";
                }
                try
                {
                    kanbanScan.TagSeqNo = Convert.ToInt32(qrCode.Substring(154, 7).Trim());
                }
                catch
                {
                    StatusMessage += "Tag Sequence No format is wrong.\n";
                }
                kanbanScan.SupplierCode = qrCode.Substring(143, 3).Trim();
            }
            else //qrCode.Length == 115
            {
                kanbanScan.PartNo = qrCode.Substring(8, 16).Trim();
                try
                {
                    kanbanScan.QtyUnit = Convert.ToDouble(qrCode.Substring(108, 7));
                }
                catch
                {
                    StatusMessage += "Lot Size format is wrong.\n";
                }
            }

            if (StatusMessage == "")
            {
                List<KanbanItem> parts = partVM.KanbanItems.Where(a => a.PartNo == kanbanScan.PartNo).ToList();
                if(parts.Count==0 || parts == null)
                {
                    StatusMessage += "Part No " + kanbanScan.PartNo + " is not available.\n";
                }
                else 
                {
                    List<KanbanItem> lotSizes = parts.Where(a => a.LotSize == kanbanScan.QtyUnit).ToList();
                    if (lotSizes.Count == 0 || lotSizes == null)
                    {
                        StatusMessage += "Part No " + kanbanScan.PartNo + " with Lot Size " + kanbanScan.QtyUnit.ToString() + " is not valid.\n";
                    }
                    else
                    {
                        List<KanbanItem> partBalance = parts.Where(a => a.Balance > 0).ToList();
                        if (partBalance.Count == 0 || partBalance == null)
                        {
                            StatusMessage += "Part No " + kanbanScan.PartNo + " required qty is already fulfilled.\n";
                        }
                    }
                }
            }

            if (StatusMessage != "")
            {
                DisplayAlert("Warning", StatusMessage, "OK");
                QRMaterialText.Text = "";
            }
            else valid = true;

            return valid;
        }
    }
}