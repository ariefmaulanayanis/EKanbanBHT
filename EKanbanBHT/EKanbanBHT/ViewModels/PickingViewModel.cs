using EKanbanBHT.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EKanbanBHT.ViewModels
{
    public class PickingViewModel: ViewModelBase
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

        private string scanText;
        public string ScanText
        {
            get => scanText;
            set
            {
                scanText = value;
                OnPropertyChanged();
            }
        }

        public ICommand ScanCommand { get; set; }

        public PickingViewModel()
        {
            ScanCommand = new Command(() =>
            {
                NavigateToPartView();
            });
        }

        private async void NavigateToPartView()
        {
            var view = Locator.Resolve<PartView>();
            var viewModel = view.BindingContext as PartViewModel;
            viewModel.EmpNo = this.EmpNo;
            viewModel.ScanText = this.ScanText;

            await Navigation.PushAsync(view);
        }
    }
}
