using EKanbanBHT.Models;
using EKanbanBHT.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EKanbanBHT.ViewModels
{
    public class PickingViewModel: ViewModelBase
    {
        //private string empNo;
        //public string EmpNo
        //{
        //    get => empNo;
        //    set
        //    {
        //        empNo = value;
        //        OnPropertyChanged();
        //    }
        //}

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

        private KanbanHeader kanbanHeader;
        public KanbanHeader KanbanHeader
        {
            get => kanbanHeader;
            set
            {
                kanbanHeader = value;
                OnPropertyChanged();
            }
        }

        private bool isAdmin;
        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                OnPropertyChanged();
            }
        }

        //private string lineName;
        //public string LineName
        //{
        //    get => lineName;
        //    set
        //    {
        //        lineName = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Int16 lineNo;
        //public Int16 LineNo
        //{
        //    get => lineNo;
        //    set
        //    {
        //        lineNo = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private DateTime requestDate;
        //public DateTime RequestDate
        //{
        //    get => requestDate;
        //    set
        //    {
        //        requestDate = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private int requestNo;
        //public int RequestNo
        //{
        //    get => requestNo;
        //    set
        //    {
        //        requestNo = value;
        //        OnPropertyChanged();
        //    }
        //}

        public ICommand ScanCommand { get; set; }
        //public ICommand SubmitCommand { get; set; }

        public PickingViewModel()
        {
            string empNo = Preferences.Get("user", "");
            IsAdmin = empNo == "999";
            KanbanHeader = new KanbanHeader();
            ScanCommand = new Command(() =>
            {
                NavigateToPartView();
            });
            //SubmitCommand = new Command(() =>
            //{
            //    SubmitTrolley();
            //});
        }

        private async void NavigateToPartView()
        {
            var view = Locator.Resolve<PartView>();
            var viewModel = view.BindingContext as PartViewModel;
            viewModel.KanbanHeader = KanbanHeader;
            //viewModel.EmpNo = this.EmpNo;
            //viewModel.ScanText = this.ScanText;

            await Navigation.PushAsync(view);
        }

        //private void SubmitTrolley()
        //{

        //}
    }
}
