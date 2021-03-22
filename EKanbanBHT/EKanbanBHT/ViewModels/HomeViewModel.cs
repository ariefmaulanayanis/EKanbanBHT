using EKanbanBHT.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EKanbanBHT.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private string empNo;
        public string EmpNo
        {
            get => empNo;
            set
            {
                empNo = value;
                OnPropertyChanged();
            }
        }

        public ICommand SignInCommand { get; set; }
        public HomeViewModel()
        {
            SignInCommand = new Command(() =>
            {
                NavigateToMenuView();
            });
            EmpNo = "";
            Preferences.Set("user", EmpNo);
        }

        private async void NavigateToMenuView()
        {
            Preferences.Set("user", EmpNo);
            var menuView = Locator.Resolve<MenuView>();
            //var menuViewModel=menuView.BindingContext as MenuViewModel;
            //menuViewModel.EmpNo = EmpNo;

            await Navigation.PushAsync(menuView);
        }
    }
}
