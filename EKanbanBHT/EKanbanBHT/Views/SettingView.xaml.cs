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

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (DeviceIdText.Text == "" || APIAddressText.Text=="")
            {
                //await DisplayAlert("Warning", "Device Id field is required.", "OK");
                await DisplayAlert("Warning", "All setting fields are required.", "OK");
            }
            else
            {
                Preferences.Set("device", DeviceIdText.Text);
                Preferences.Set("api", APIAddressText.Text);
                //await DisplayAlert("Save Success", "Device Id has been saved.", "OK");
                await DisplayAlert("Save Success", "All setting fields have been saved.", "OK");
            }
        }
    }
}