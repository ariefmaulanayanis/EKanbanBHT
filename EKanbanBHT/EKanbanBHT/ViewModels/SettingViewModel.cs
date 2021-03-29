using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;

namespace EKanbanBHT.ViewModels
{
    public class SettingViewModel:ViewModelBase
    {
        private string deviceId;
        public string DeviceId
        {
            get => deviceId;
            set
            {
                deviceId = value;
                OnPropertyChanged();
            }
        }

        private string apiAddress;
        public string APIAddress
        {
            get => apiAddress;
            set
            {
                apiAddress = value;
                OnPropertyChanged();
            }
        }

        private string ftpHost;
        public string FTPHost
        {
            get => ftpHost;
            set
            {
                ftpHost = value;
                OnPropertyChanged();
            }
        }

        private string ftpUser;
        public string FTPUser
        {
            get => ftpUser;
            set
            {
                ftpUser = value;
                OnPropertyChanged();
            }
        }

        private string ftpPassword;
        public string FTPPassword
        {
            get => ftpPassword;
            set
            {
                ftpPassword = value;
                OnPropertyChanged();
            }
        }

        private string ftpPort;
        public string FTPPort
        {
            get => ftpPort;
            set
            {
                ftpPort = value;
                OnPropertyChanged();
            }
        }

        public SettingViewModel()
        {
            DeviceId = Preferences.Get("device", "");
            APIAddress = Preferences.Get("api", "");
            FTPHost = Preferences.Get("ftpHost", "");
            FTPUser = Preferences.Get("ftpUser", "");
            FTPPassword = Preferences.Get("ftpPassword", "");
            FTPPort = Preferences.Get("ftpPort", "");
        }
    }
}
