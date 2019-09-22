using CophiPoint.Api;
using CophiPoint.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly AuthService _auth;

        public LoginPage()
        {
            _auth = ((App)Application.Current).AuthService;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_auth.IsLoggedIn)
            {
                await Loading(OpenMain);
            }
        }

        private async void ShowInfo(object sender, EventArgs e)
        {
            var page = await HtmlPage.GetInfoPage();
            await Navigation.PushAsync(page);
        }
        private async void RequestLogin(object sender, EventArgs e)
        {
            await Loading(Login);
        }
        private async Task Login()
        {
            var result = await _auth.Login();
            if (result.IsSucessful)
            {
                await OpenMain();
            }
            else
            {
                await DisplayAlert(GeneralResources.LoginFailedAlert , result.Error, GeneralResources.AlertCancel);
            }
        }
        private async Task Loading(Func<Task> func)
        {

            activity.IsRunning = true;
            activity.IsVisible = true;
            try
            {
                await func();
            }
            finally
            {
                activity.IsVisible = false;
                activity.IsRunning = false;
            }
        }
        
        private async Task OpenMain()
        {
            await ((App)Application.Current).Reload();
            await Navigation.PushAsync(new MainPage(), false);
        }
    }
}