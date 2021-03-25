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
    public partial class PartView : ContentPage
    {
        PartViewModel partVM { get; set; }
        public PartView(PartViewModel viewModel)
        {
            InitializeComponent();

            partVM = viewModel;
            partVM.Navigation = Navigation;
            BindingContext = partVM;
        }

        protected override bool OnBackButtonPressed()
        {
            var view = Locator.Resolve<PickingView>();
            var viewModel = view.BindingContext as PickingViewModel;
            viewModel.ScanText = "";

            Navigation.PushAsync(view);
            return true;
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(QRMaterialText.Text))
                await DisplayAlert("Warning", "You have to fill Scan Material.", "OK");
            else if (QRMaterialText.Text.Length != 223 && QRMaterialText.Text.Length != 115)
                await DisplayAlert("Warning", "Wrong format Scan Result (length is not 223 or 115)", "OK");
            else
            {
                string partNo = "";
                if (QRMaterialText.Text.Length == 223) partNo = QRMaterialText.Text.Substring(96, 15).Trim();
                if (QRMaterialText.Text.Length == 115) partNo = QRMaterialText.Text.Substring(8, 16).Trim();
                List<KanbanItem> items = partVM.KanbanItems.Where(a => a.PartNo == partNo).ToList();
                if (items.Count == 0 || items==null)
                {
                    await DisplayAlert("Warning", "There's no part no " + partNo + " in the item list", "OK");
                }
                else
                {
                    bool canAdd=false;
                    int i = 0;
                    foreach (KanbanItem item in items)
                    {
                        if (item.Balance > 0)
                        {
                            item.ScanQty++;
                            item.Balance--;
                            canAdd = true;
                            i++;
                            break;
                        }
                    }
                    if(!canAdd) 
                        await DisplayAlert("Warning", "Can't add part " + partNo + ", the required qty has been fulfilled.", "OK");

                }
            }
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {

        }

        private void QRMaterialText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}