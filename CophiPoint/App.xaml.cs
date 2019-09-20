using CophiPoint.Pages;
using CophiPoint.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            var authService = new AuthService();
            var page = (authService.IsLoggedIn) ? (Page)new MainPage() : new LoginPage(authService);
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
