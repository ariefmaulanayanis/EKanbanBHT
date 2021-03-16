﻿using EKanbanBHT.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EKanbanBHT.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickingView : ContentPage
    {
        public PickingView(PickingViewModel viewModel)
        {
            InitializeComponent();

            viewModel.Navigation = Navigation;
            BindingContext = viewModel;
        }
    }
}