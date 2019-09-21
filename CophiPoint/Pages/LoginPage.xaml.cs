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

        private async void ShowInfo(object sender, EventArgs e)
        {
            var page = await HtmlPage.FromUrl(Urls.InfoPage);
            await Navigation.PushAsync(page);
        }
        private async void RequestLogin(object sender, EventArgs e)
        {
            await _auth.Login();
        }
    }
}