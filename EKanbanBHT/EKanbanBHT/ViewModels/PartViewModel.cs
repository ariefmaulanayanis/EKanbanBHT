using EKanbanBHT.Models;
using System;
using System.Collections.Generic;
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

        private List<KanbanItem> kanbanItems;
        public List<KanbanItem> KanbanItems
        {
            get => kanbanItems;
            set
            {
                kanbanItems = value;
                OnPropertyChanged();
            }
        }

        public PartViewModel()
        {
            string empNo = Preferences.Get("user", "");
            IsAdmin = empNo == "999";
            KanbanHeader = new KanbanHeader();
            KanbanItems = new List<KanbanItem>();
            kanbanItemRepo = new KanbanItemRepository();
        }
    }
}
