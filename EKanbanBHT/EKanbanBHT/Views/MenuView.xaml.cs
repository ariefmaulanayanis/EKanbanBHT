using EKanbanBHT.Models;
using EKanbanBHT.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
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
                await DisplayAlert("Warning", "API Address is empty, please entry it in setting menu.", "OK");
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

        private async void UploadButton_Clicked(object sender, EventArgs e)
        {
            if (menuVM.IsBusy) return;

            string FTPHost = Preferences.Get("ftpHost", "");
            string FTPUser = Preferences.Get("ftpUser", "");
            string FTPPassword = Preferences.Get("ftpPassword", "");
            string FTPPort = Preferences.Get("ftpPort", "");
            if (string.IsNullOrEmpty(FTPHost) || string.IsNullOrEmpty(FTPUser) || 
                string.IsNullOrEmpty(FTPPassword) || string.IsNullOrEmpty(FTPPort))
            {
                menuVM.IsBusy = false;
                await DisplayAlert("Warning", "FTP setting is not complete, please entry it in setting menu.", "OK");
            }
            else
            {
                try
                {
                    string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "file.txt");

                    //copy file
                    //string destination = "ftp://files.000webhost.com/tmp/file.txt";
                    string destination = FTPHost + ":" + FTPPort + "/tmp/file.txt";
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(destination);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    //request.UseBinary = true;
                    //request.UsePassive = false;//true;
                    //request.KeepAlive = false;
                    //request.Timeout = System.Threading.Timeout.Infinite;
                    //request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;
                    //request.Credentials = new NetworkCredential("alhijrahshop123", "maniez1982");
                    request.Credentials = new NetworkCredential(FTPUser, FTPPassword);

                    StreamReader sourceStream = new StreamReader(file);
                    byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                    sourceStream.Close();
                    request.ContentLength = fileContents.Length;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(fileContents, 0, fileContents.Length);
                    requestStream.Close();
                    requestStream.Dispose();

                    //
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    response.Close();
                    response.Dispose();
                    menuVM.IsBusy = false;
                }
                catch (Exception ex)
                {
                    menuVM.IsBusy = false;
                    await DisplayAlert("Warning", ex.Message, "OK");
                }
            }
        }
    }
}