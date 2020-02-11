using CophiPoint.Api;
using CophiPoint.Services;
using CophiPoint.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly AuthService _authService;
        private readonly HtmlManager _htmlService;

        public LoginPage(AuthService authService, HtmlManager htmlService)
        {
            _authService = authService;
            _htmlService = htmlService;
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_authService.IsLoggedIn)
            {
                await Loading(OpenMain);
            }
        }

        private async void ShowInfo(object sender, EventArgs e)
        {
            var page = await _htmlService.GetPageAsync(GeneralResources.InfoTitle, u => u.General);
            await Navigation.PushAsync(page);
        }
        private async void RequestLogin(object sender, EventArgs e)
        {
            await Loading(Login);
        }
        private async Task Login()
        {
            var result = await _authService.Login();
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
            await Navigation.PushAsync(TinyIoCContainer.Current.Resolve<MainPage>(), false);
        }
    }
}