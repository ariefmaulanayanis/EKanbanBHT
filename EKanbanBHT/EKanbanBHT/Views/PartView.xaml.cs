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
            ScannedArea.IsVisible = false;
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

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            List<KanbanItem> parts = partVM.KanbanItems.Where(a => a.Balance > 0).ToList();
            bool valid = true;
            bool isCompleted = true;
            if (parts.Count > 0)
            {
                //await DisplayAlert("Warning", "There's still parts not fulfilled.", "OK");
                isCompleted = false;
                valid = await DisplayAlert("Warning", "Kanban Picking is not completed yet, do you want to save as draft?", "Yes", "No");
            }
            if(valid)
            {
                partVM.KanbanHeader.PickEnd = DateTime.Now;
                partVM.KanbanHeader.PickerName = Preferences.Get("user", "");
                kanbanItemRepo.KanbanSave(partVM.KanbanHeader.KanbanReqId, partVM.KanbanHeader.PickStart.Value, isCompleted);
                //kanbanItemRepo.KanbanScanSave(scanList);
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
            if (qrCode == "") return;
            if (ValidateQRCode(qrCode))
            {
                int i = 0;
                bool isFound = false;
                foreach (KanbanItem item in partVM.KanbanItems.ToArray())
                {
                    if (item.PartNo == kanbanScan.PartNo && item.Balance > 0 && isFound==false)
                    {
                        //update kanban item
                        partVM.KanbanItems[i].Balance--;
                        partVM.KanbanItems[i].ScanQty++;
                        partVM.KanbanItems[i].BackgroundColor = "Orange";
                        isFound = true;
                        KanbanList.ItemsSource = null;
                        KanbanList.ItemsSource = partVM.KanbanItems;
                        kanbanItemRepo.UpdateKanbanItem(partVM.KanbanItems[i]); //langsung simpan ke table

                        //update kanban scan
                        kanbanScan.ReqItemId = item.ReqItemId;
                        kanbanScan.KanbanReqId = item.KanbanReqId;
                        if (char.IsNumber(kanbanScan.PartNo[0]))
                        {
                            kanbanScan.TagSeqNo = i + 1;
                            kanbanScan.SupplierCode = "A00";
                        }
                        kanbanScan.TagDataCode = char.IsNumber(kanbanScan.PartNo[0]) ? "10" : "11";
                        kanbanScan.DeviceId = Preferences.Get("device", "");
                        kanbanScan.EmpNo = Preferences.Get("user", "");
                        kanbanItemRepo.KanbanScanSave(kanbanScan);
                        partVM.KanbanScans = kanbanItemRepo.GetKanbanScan(item.KanbanReqId);

                        //scanList.Add(kanbanScan);

                        ScannedArea.IsVisible = true;
                        partVM.ScannedKanban = partVM.KanbanItems[i];

                        //break;
                    }
                    else
                    {
                        partVM.KanbanItems[i].BackgroundColor = "AntiqueWhite";
                        kanbanItemRepo.UpdateKanbanItem(partVM.KanbanItems[i]); //langsung simpan ke table
                    }
                    i++;
                }
            }
            QRMaterialText.Text = "";
        }

        private bool ValidateQRCode(string qrCode)
        {
            bool valid = false;
            string StatusMessage = "";
            partVM.PartNo = "";
            kanbanScan = new KanbanScan();
            kanbanScan.ScanDateTime = DateTime.Now;

            if (qrCode.Length != 115 && qrCode.Length != 223)
            {
                //qr code is not valid
                StatusMessage += "QC Code format is not valid.\n";
            }
            else
            {
                //parsing string dan validasi format
                kanbanScan.QRLength = qrCode.Length;
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
                    kanbanScan.SupplierCode = "A00";
                }
            }

            if (StatusMessage == "")
            {
                //jika part no tidak terdapat dalam kanban
                List<KanbanItem> parts = partVM.KanbanItems.Where(a => a.PartNo == kanbanScan.PartNo).ToList();
                if(parts.Count==0 || parts == null)
                {
                    StatusMessage += "Part No " + kanbanScan.PartNo + " is not available.\n";
                }
                else 
                {
                    //jika lot size berbeda
                    List<KanbanItem> lotSizes = parts.Where(a => a.LotSize == kanbanScan.QtyUnit).ToList();
                    if (lotSizes.Count == 0 || lotSizes == null)
                    {
                        StatusMessage += "Part No " + kanbanScan.PartNo + " with Lot Size " + kanbanScan.QtyUnit.ToString() + " is not valid.\n";
                    }

                    //jika part yg discan sudah cukup
                    if (StatusMessage == "")
                    {
                        List<KanbanItem> partBalance = parts.Where(a => a.Balance > 0).ToList();
                        if (partBalance.Count == 0 || partBalance == null)
                        {
                            StatusMessage += "Part No " + kanbanScan.PartNo + " required qty is already fulfilled.\n";
                        }
                    }

                    //jika tag seq no sudah ada
                    if (StatusMessage == "" && qrCode.Length == 223)
                    {
                        List<KanbanScan> scanList = partVM.KanbanScans.Where(a => a.PartNo == kanbanScan.PartNo && a.TagSeqNo == kanbanScan.TagSeqNo).ToList();
                        if (scanList.Count > 0)
                        {
                            StatusMessage += "Part No " + kanbanScan.PartNo + " with Tag Seq No " + kanbanScan.TagSeqNo.ToString() + " has already been scanned.\n";
                        }
                    }
                }
            }

            if (StatusMessage != "")
            {
                DisplayAlert("Warning", StatusMessage, "OK");
                QRMaterialText.Text = "";
                ScannedArea.IsVisible = false;
            }
            else valid = true;

            return valid;
        }
    }
}