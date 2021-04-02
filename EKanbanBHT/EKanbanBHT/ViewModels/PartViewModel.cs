using EKanbanBHT.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EKanbanBHT.ViewModels
{
    public class PartViewModel : ViewModelBase
    {
        public static KanbanItemRepository kanbanItemRepo { get; private set; }
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

        private ObservableCollection<KanbanItem> kanbanItems;
        public ObservableCollection<KanbanItem> KanbanItems
        {
            get => kanbanItems;
            set
            {
                kanbanItems = value;
                OnPropertyChanged();
            }
        }

        private List<KanbanScan> kanbanScans;
        public List<KanbanScan> KanbanScans
        {
            get => kanbanScans;
            set
            {
                kanbanScans = value;
                OnPropertyChanged();
            }
        }

        private string partNo;
        public string PartNo
        {
            get => partNo;
            set
            {
                partNo = value;
                OnPropertyChanged();
            }
        }

        public PartViewModel()
        {
            string empNo = Preferences.Get("user", "");
            IsAdmin = empNo == "999";
            KanbanHeader = new KanbanHeader();
            KanbanItems = new ObservableCollection<KanbanItem>();
            kanbanItemRepo = new KanbanItemRepository();
        }
    }
}
