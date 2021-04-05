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
    public partial class SettingView : ContentPage
    {
        public SettingView(SettingViewModel viewModel)
        {
            InitializeComponent();

            viewModel.Navigation = Navigation;
            BindingContext = viewModel;
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            Save();
        }

        private void FTPPort_Completed(object sender, EventArgs e)
        {
            Save();
        }

        private async void Save()
        {
            if (DeviceIdText.Text == "" || APIAddressText.Text == "" ||
                FTPHost.Text == "" || FTPUser.Text == "" ||
                FTPPassword.Text == "" || FTPPort.Text == "")
            {
                await DisplayAlert("Warning", "All setting fields are required.", "OK");
            }
            else
            {
                Preferences.Set("device", DeviceIdText.Text);
                Preferences.Set("api", APIAddressText.Text);
                Preferences.Set("ftpHost", FTPHost.Text);
                Preferences.Set("ftpUser", FTPUser.Text);
                Preferences.Set("ftpPassword", FTPPassword.Text);
                Preferences.Set("ftpPort", FTPPort.Text);

                await DisplayAlert("Save Success", "All setting fields have been saved.", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}