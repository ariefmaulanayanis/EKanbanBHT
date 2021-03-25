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

        public SettingViewModel()
        {
            DeviceId = Preferences.Get("device", "");
            APIAddress = Preferences.Get("api", "");
        }
    }
}
