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
        public PartView(PartViewModel viewModel)
        {
            InitializeComponent();

            viewModel.Navigation = Navigation;
            BindingContext = viewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            var view = Locator.Resolve<PickingView>();
            var viewModel = view.BindingContext as PickingViewModel;
            viewModel.ScanText = "";

            Navigation.PushAsync(view);
            return true;
        }
    }
}