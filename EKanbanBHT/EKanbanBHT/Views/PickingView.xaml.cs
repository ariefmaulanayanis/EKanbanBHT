using EKanbanBHT.Models;
using EKanbanBHT.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EKanbanBHT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickingView : ContentPage
    {
        List<KanbanItem> kanbanItems;
        public static KanbanItemRepository kanbanItemRepo { get; private set; }
        PickingViewModel pickingVM { get; set; }
        public PickingView(PickingViewModel viewModel)
        {
            InitializeComponent();

            kanbanItemRepo = new KanbanItemRepository();
            pickingVM = viewModel;
            pickingVM.Navigation = Navigation;
            BindingContext = pickingVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            QRCodeText.Focus();
        }

        //private async void SubmitButton_Clicked(object sender, EventArgs e)
        //{            
        //    if(string.IsNullOrEmpty(QRCodeText.Text))
        //        await DisplayAlert("Warning", "You have to fill Scan Result.", "OK");
        //    else if(QRCodeText.Text.Length!=49)
        //        await DisplayAlert("Warning", "Wrong format Scan Result (length is not 49)", "OK");
        //    else
        //    {
        //        string StatusMessage = "";
        //        pickingVM.KanbanHeader.LineName = QRCodeText.Text.Substring(0, 30).Trim();
        //        try
        //        {
        //            pickingVM.KanbanHeader.LineNo = Convert.ToInt16(QRCodeText.Text.Substring(30, 2));
        //        }
        //        catch
        //        {
        //            StatusMessage += "Line No format is wrong.\n";
        //        }
        //        try
        //        {
        //            string year = QRCodeText.Text.Substring(32, 4);
        //            string month = QRCodeText.Text.Substring(36, 2);
        //            string date = QRCodeText.Text.Substring(38, 2);
        //            string hour = QRCodeText.Text.Substring(40, 2);
        //            string minute = QRCodeText.Text.Substring(42, 2);
        //            string second = QRCodeText.Text.Substring(44, 2);
        //            string formattedDate = year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;

        //            pickingVM.KanbanHeader.RequestDate = Convert.ToDateTime(formattedDate);
        //        }
        //        catch
        //        {
        //            StatusMessage += "Request Date format is wrong.\n";
        //        }
        //        try
        //        {
        //            pickingVM.KanbanHeader.RequestNo = Convert.ToInt32(QRCodeText.Text.Substring(46, 3));
        //        }
        //        catch
        //        {
        //            StatusMessage += "Request No format is wrong.\n";
        //        }
        //        if(StatusMessage!="")
        //            await DisplayAlert("Warning", StatusMessage, "OK");
        //        else
        //        {
        //            var view = Locator.Resolve<PartView>();
        //            var viewModel = view.BindingContext as PartViewModel;
        //            KanbanHeader kanbanHeader= pickingVM.KanbanHeader;
        //            viewModel.KanbanHeader = kanbanHeader;
        //            viewModel.KanbanItems = kanbanItemRepo.GetKanbanItems(kanbanHeader.RequestDate, kanbanHeader.RequestNo);
        //            await Navigation.PushAsync(view);
        //        }
        //    }
        //}

        private async void QRCodeText_Completed(object sender, EventArgs e)
        {
            await DisplayAlert("Warning", ((Entry)sender).Text, "OK");
        }

        /// <summary>
        /// Allows you to pass the scanned data form the ScannerCode.cs to the TextView on this Activity
        /// </summary>
        /// <param name="Result"></param>
        public void On_Result_Event(string Result)//Changed Scan Result event from static instance
        {
            Task.Run(() => {//Changed Invoke UI Thread
                QRCodeText.Text = Result;
            });
        }

        private void QRCodeText_TextChanged(object sender, TextChangedEventArgs e)
        {
            string qrCode = e.NewTextValue;
            //if(qrCode.Length==49) ((IEntryController)sender).SendCompleted();
            if (qrCode.Length == 49)
            {
                if (ValidateQRCode(qrCode))
                {
                    var view = Locator.Resolve<PartView>();
                    var viewModel = view.BindingContext as PartViewModel;
                    viewModel.KanbanHeader = pickingVM.KanbanHeader;
                    viewModel.KanbanItems = kanbanItems;
                    Navigation.PushAsync(view);
                }
            }
        }

        private bool ValidateQRCode(string qrCode)
        {
            bool valid = false;
            string StatusMessage = "";
            pickingVM.KanbanHeader.LineName = QRCodeText.Text.Substring(0, 30).Trim();
            try
            {
                pickingVM.KanbanHeader.LineNo = Convert.ToInt16(QRCodeText.Text.Substring(30, 2));
            }
            catch
            {
                StatusMessage += "Line No format is wrong.\n";
            }
            try
            {
                string year = QRCodeText.Text.Substring(32, 4);
                string month = QRCodeText.Text.Substring(36, 2);
                string date = QRCodeText.Text.Substring(38, 2);
                string hour = QRCodeText.Text.Substring(40, 2);
                string minute = QRCodeText.Text.Substring(42, 2);
                string second = QRCodeText.Text.Substring(44, 2);
                string formattedDate = year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;

                pickingVM.KanbanHeader.RequestDate = Convert.ToDateTime(formattedDate);
            }
            catch
            {
                StatusMessage += "Request Date format is wrong.\n";
            }
            try
            {
                pickingVM.KanbanHeader.RequestNo = Convert.ToInt32(QRCodeText.Text.Substring(46, 3));
            }
            catch
            {
                StatusMessage += "Request No format is wrong.\n";
            }
            kanbanItems = new List<KanbanItem>();
            if (StatusMessage == "")
            {
                kanbanItems = kanbanItemRepo.GetKanbanItems(pickingVM.KanbanHeader.RequestDate, pickingVM.KanbanHeader.RequestNo);
                if(kanbanItems.Count==0 || kanbanItems == null)
                {
                    StatusMessage += "Kanban No " + pickingVM.KanbanHeader.RequestNo.ToString() + " requested on " + pickingVM.KanbanHeader.RequestDate.ToString("dd-MMM-yyyy") + " is not available.\n";
                }
            }
            if (StatusMessage != "") DisplayAlert("Warning", StatusMessage, "OK");
            else valid = true;
            return valid;
        }
    }
}