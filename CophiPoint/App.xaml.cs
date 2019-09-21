using CophiPoint.Pages;
using CophiPoint.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint
{
    public partial class App : Application
    {
        public AuthService AuthService { get; }
        public TestRestService RestService { get; }

        public App()
        {
            InitializeComponent();
            
            AuthService = new AuthService();
            RestService = new TestRestService();
            var page = (AuthService.IsLoggedIn) ? (Page)new MainPage() : new LoginPage();
            MainPage = new NavigationPage(page);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
