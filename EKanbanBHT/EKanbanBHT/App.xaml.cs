using EKanbanBHT.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EKanbanBHT
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            Locator.Initialize();

            InitializeRepository();
            InitializeMainPage();
        }

        private void InitializeMainPage()
        {
            MainPage = new NavigationPage(Locator.Resolve<HomeView>());
        }

        private static void InitializeRepository()
        {
            //INoteRepository repository = Locator.Resolve<INoteRepository>();
            //repository.Initialize();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
