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
    public partial class MenuView : ContentPage
    {
        public MenuView(MenuViewModel menuViewModel)
        {
            InitializeComponent();

            menuViewModel.Navigation = Navigation;
            BindingContext = menuViewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            var view = Locator.Resolve<HomeView>();
            var viewModel = view.BindingContext as HomeViewModel;
            viewModel.EmpNo = "";

            Navigation.PushAsync(view);
            return true;
            //return base.OnBackButtonPressed();
            //return new NavigationPage(new HomeView());
        }
    }
}