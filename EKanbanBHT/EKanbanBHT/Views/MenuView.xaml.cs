using EKanbanBHT.Models;
using EKanbanBHT.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EKanbanBHT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : ContentPage
    {
        public static KanbanItemRepository kanbanItemRepo { get; private set; }
        //readonly IList<KanbanItem> kanbanItems = new ObservableCollection<KanbanItem>();
        readonly KanbanManager manager = new KanbanManager();
        private MenuViewModel menuVM { get; set; }

        public MenuView(MenuViewModel menuViewModel)
        {
            InitializeComponent();

            kanbanItemRepo = new KanbanItemRepository();
            menuViewModel.IsBusy = false;
            menuViewModel.Navigation = Navigation;
            menuVM = menuViewModel;
            BindingContext = menuViewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            var view = Locator.Resolve<HomeView>();
            var viewModel = view.BindingContext as HomeViewModel;
            //viewModel.EmpNo = "";

            Navigation.PushAsync(view);
            return true;
            //return base.OnBackButtonPressed();
            //return new NavigationPage(new HomeView());
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (menuVM.IsBusy) return;

            var result = await DisplayAlert("Delete Confirmation", "Are you sure to delete old data (> 1 month)", "Yes", "No");
            if (result)
            {
                menuVM.IsBusy = true;

                kanbanItemRepo.DeleteOldData();

                menuVM.IsBusy = false;
                if (!string.IsNullOrEmpty(kanbanItemRepo.StatusMessage))
                {
                    if (kanbanItemRepo.IsError)
                        await DisplayAlert("Delete Fail", kanbanItemRepo.StatusMessage, "OK");
                    else
                        await DisplayAlert("Delete Success", kanbanItemRepo.StatusMessage, "OK");
                }
            }
        }

        private async void SyncButton_Clicked(object sender, EventArgs e)
        {
            if (menuVM.IsBusy) return;

            string APIAddress = Preferences.Get("api", "");
            if (string.IsNullOrEmpty(APIAddress))
            {
                await DisplayAlert("Sync Data Fail", "API Address is empty, please entry it in setting menu.", "OK");
            }
            else
            {
                List<KanbanItem> kanbanList = new List<KanbanItem>();
                menuVM.IsBusy = true;
                try
                {
                    kanbanList = await manager.GetAll();
                }
                catch (Exception ex)
                {
                    menuVM.IsBusy = false;
                    await DisplayAlert("Sync Data Fail", ex.Message, "OK");
                    return;
                }

                kanbanItemRepo.SyncData(kanbanList);
                menuVM.IsBusy = false;
                if (!string.IsNullOrEmpty(kanbanItemRepo.StatusMessage))
                {
                    if(kanbanItemRepo.IsError)
                        await DisplayAlert("Sync Data Fail", kanbanItemRepo.StatusMessage, "OK");
                    else
                        await DisplayAlert("Sync Data Success", kanbanItemRepo.StatusMessage, "OK");
                }
            }

        }

        private async void DeleteAllButton_Clicked(object sender, EventArgs e)
        {
            if (menuVM.IsBusy) return;

            var result = await DisplayAlert("Delete Confirmation", "Are you sure to delete all data?", "Yes", "No");
            if (result)
            {
                menuVM.IsBusy = true;

                kanbanItemRepo.DeleteAllData();

                menuVM.IsBusy = false;
                if (!string.IsNullOrEmpty(kanbanItemRepo.StatusMessage))
                {
                    if (kanbanItemRepo.IsError)
                        await DisplayAlert("Delete Fail", kanbanItemRepo.StatusMessage, "OK");
                    else
                        await DisplayAlert("Delete Success", kanbanItemRepo.StatusMessage, "OK");
                }
            }
        }
    }
}