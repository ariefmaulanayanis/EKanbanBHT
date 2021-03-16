using EKanbanBHT.ViewModels;
using EKanbanBHT.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EKanbanBHT
{
    public static class Locator
    {
        private static IServiceProvider serviceProvider;

        public static void Initialize()
        {
            var services = new ServiceCollection();
            //services.AddSingleton<INoteRepository, SqliteNoteRepository>();
            //add view model
            services.AddTransient<HomeViewModel>();
            services.AddTransient<MenuViewModel>();
            services.AddTransient<PickingViewModel>();
            services.AddTransient<UploadViewModel>();
            services.AddTransient<SyncViewModel>();
            services.AddTransient<DeleteViewModel>();
            services.AddTransient<SettingViewModel>();
            services.AddTransient<PartViewModel>();
            //add view view
            services.AddTransient<HomeView>();
            services.AddTransient<MenuView>();
            services.AddTransient<PickingView>();
            services.AddTransient<UploadView>();
            services.AddTransient<SyncView>();
            services.AddTransient<DeleteView>();
            services.AddTransient<SettingView>();
            services.AddTransient<PartView>();

            serviceProvider = services.BuildServiceProvider();
        }

        public static T Resolve<T>() => serviceProvider.GetService<T>();
    }
}
