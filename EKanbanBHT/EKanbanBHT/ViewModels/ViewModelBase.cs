using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EKanbanBHT.ViewModels
{
    public class ViewModelBase:BindableObject
    {
        public INavigation Navigation { get; set; }
    }
}
