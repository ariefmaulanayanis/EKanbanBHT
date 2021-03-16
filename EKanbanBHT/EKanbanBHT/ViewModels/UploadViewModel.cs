using System;
using System.Collections.Generic;
using System.Text;

namespace EKanbanBHT.ViewModels
{
    public class UploadViewModel:ViewModelBase
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
    }
}
