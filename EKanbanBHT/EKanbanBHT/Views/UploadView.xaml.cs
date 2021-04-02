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
    public partial class UploadView : ContentPage
    {
        UploadViewModel uploadVM { get; set; }
        public UploadView(UploadViewModel viewModel)
        {
            InitializeComponent();

            uploadVM = viewModel;
            uploadVM.Navigation = Navigation;
            BindingContext = uploadVM;

        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    uploadVM.IsBusy = true;
        //    uploadVM.UploadDatFiles();
        //    if (uploadVM.StatusMessage != "")
        //    {
        //        uploadVM.IsBusy = false;
        //        DisplayAlert("Warning", uploadVM.StatusMessage, "OK");
        //    }
        //    else
        //    {
        //        DisplayAlert("Upload Success", "All saved data have been uploaded.", "OK");                
        //    }
        //    //redirect to menu
        //    var view = Locator.Resolve<MenuView>();
        //    Navigation.PushAsync(view);
        //}

        private void UploadButton_Clicked(object sender, EventArgs e)
        {
            uploadVM.IsBusy = true;
            uploadVM.UploadDatFiles();
            uploadVM.IsBusy = false;
            if (uploadVM.StatusMessage != "")
            {
                DisplayAlert("Warning", uploadVM.StatusMessage, "OK");
            }
            else
            {
                DisplayAlert("Upload Success", "All saved kanban(s) have been uploaded.", "OK");
            }
        }
    }
}