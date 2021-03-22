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

        public MenuView(MenuViewModel menuViewModel)
        {
            InitializeComponent();

            kanbanItemRepo = new KanbanItemRepository();
            menuViewModel.Navigation = Navigation;
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
            if (IsBusy) return;

            IsBusy = true;
            var result = await DisplayAlert("Delete Confirmation", "Are you sure to delete old data (> 1 month)", "Yes", "No");
            if (result)
            {
                kanbanItemRepo.DeleteOldData();
                await DisplayAlert("Delete Success", kanbanItemRepo.StatusMessage, "OK");
            }

            IsBusy = false;
        }

        private async void SyncButton_Clicked(object sender, EventArgs e)
        {
            if (IsBusy) return;

            IsBusy = true;
            List<KanbanItem> kanbanList = await manager.GetAll();
            kanbanItemRepo.SyncData(kanbanList);
            if (!string.IsNullOrEmpty(kanbanItemRepo.StatusMessage))
                await DisplayAlert("Sync Data Success", kanbanItemRepo.StatusMessage, "OK");

            IsBusy = false;
        }
    }
}