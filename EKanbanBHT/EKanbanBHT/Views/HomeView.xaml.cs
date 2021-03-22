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
    public partial class HomeView : ContentPage
    {
        public HomeView(HomeViewModel homeViewModel)
        {
            InitializeComponent();

            Preferences.Remove("user");
            homeViewModel.Navigation = Navigation;
            //homeViewModel.EmpNo = "";
            BindingContext = homeViewModel;
        }

        protected override void OnAppearing()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Navigation = Navigation;
            //EmpNo.Text = "";
            //homeViewModel.EmpNo = "";
            BindingContext = homeViewModel;
            base.OnAppearing();
        }
    }
}