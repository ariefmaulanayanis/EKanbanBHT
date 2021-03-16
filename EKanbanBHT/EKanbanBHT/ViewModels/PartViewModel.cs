using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EKanbanBHT.ViewModels
{
    public class PartViewModel : ViewModelBase
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

        public PartViewModel()
        {
        }
    }
}
