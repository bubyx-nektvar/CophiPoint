using CophiPoint.Api;
using CophiPoint.Pages;
using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryHeaderView : ContentView
    {

        public static readonly BindableProperty ShownProperty = BindableProperty.Create(nameof(Shown), typeof(bool?), typeof(HistoryHeaderView), false, BindingMode.TwoWay);

        public bool? Shown
        {
            get => (bool?)GetValue(ShownProperty);
            set => SetValue(ShownProperty, value);
        }

        public HistoryHeaderView()
        {
            InitializeComponent();
            BindingContext = new UserViewModel
            {
                Balance = -1123.21m,
                User = "filip.havel@mojeaplikace.com",
            };
        }

        private async void OpenPaymentInfo(object sender, EventArgs e)
        {
            var page = await HtmlPage.FromUrl(Urls.PaymentPage);
            await Navigation.PushAsync(page);
        }

        private async void LogOut(object sender, EventArgs e)
        {

        }
    }
}