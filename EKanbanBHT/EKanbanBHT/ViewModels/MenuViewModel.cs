using EKanbanBHT.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EKanbanBHT.ViewModels
{
    public class MenuViewModel : ViewModelBase
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

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                IsEnabled = !isBusy;
                OnPropertyChanged();
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand PickingCommand { get; set; }
        public ICommand UploadCommand { get; set; }
        //public ICommand SyncCommand { get; set; }
        //public ICommand DeleteCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public MenuViewModel()
        {
            EmpNo = Preferences.Get("user", "");
            IsAdmin = EmpNo == "999";
            PickingCommand = new Command(() =>
            {
                NavigateToPickingView();
            });
            UploadCommand = new Command(() =>
            {
                NavigateToUploadView();
            });
            //SyncCommand = new Command(() =>
            //{
            //    NavigateToSyncView();
            //});
            //DeleteCommand = new Command(() =>
            //{
            //    NavigateToDeleteView();
            //});
            SettingCommand = new Command(() =>
            {
                NavigateToSettingView();
            });
            BackCommand = new Command(() =>
            {
                NavigateToHomeView();
                //Shell.Current.GoToAsync("///HomeView");
            });
        }

        private async void NavigateToPickingView()
        {
            var view = Locator.Resolve<PickingView>();
            var viewModel = view.BindingContext as PickingViewModel;
            //viewModel.EmpNo = this.EmpNo;

            await Navigation.PushAsync(view);
        }

        private async void NavigateToUploadView()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "file.txt");
            File.WriteAllText(file, "ini adalah content.");

            var view = Locator.Resolve<UploadView>();
            var viewModel = view.BindingContext as UploadViewModel;
            //viewModel.EmpNo = this.EmpNo;

            await Navigation.PushAsync(view);
        }

        //private async void NavigateToSyncView()
        //{
        //    var view = Locator.Resolve<SyncView>();
        //    var viewModel = view.BindingContext as SyncViewModel;
        //    //viewModel.EmpNo = this.EmpNo;

        //    await Navigation.PushAsync(view);
        //}

        //private async void NavigateToDeleteView()
        //{
        //    var view = Locator.Resolve<DeleteView>();
        //    var viewModel = view.BindingContext as DeleteViewModel;
        //    //viewModel.EmpNo = this.EmpNo;

        //    await Navigation.PushAsync(view);
        //}

        private async void NavigateToSettingView()
        {
            var view = Locator.Resolve<SettingView>();
            var viewModel = view.BindingContext as SettingViewModel;
            //viewModel.EmpNo = this.EmpNo;

            await Navigation.PushAsync(view);
        }

        private async void NavigateToHomeView()
        {
            var view = Locator.Resolve<HomeView>();
            var viewModel = view.BindingContext as HomeViewModel;
            //viewModel.EmpNo = "";

            await Navigation.PushAsync(view);
        }
    }
}
