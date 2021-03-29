using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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

        private string content;
        public string Content
        {
            get => content;
            set
            {
                content = value;
                OnPropertyChanged();
            }
        }


        public UploadViewModel()
        {
            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "file.txt");
            Content = File.ReadAllText(file);

        }
    }
}
