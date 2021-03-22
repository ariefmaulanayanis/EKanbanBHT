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

        public SettingViewModel()
        {
            DeviceId = Preferences.Get("device", "");
        }
    }
}
