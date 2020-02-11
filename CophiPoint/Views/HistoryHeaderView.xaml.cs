using CophiPoint.Api;
using CophiPoint.Pages;
using CophiPoint.Services;
using CophiPoint.Services.Implementation;
using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryHeaderView : ContentView
    {

        public static readonly BindableProperty ShownProperty = BindableProperty.Create(nameof(Shown), typeof(bool?), typeof(HistoryHeaderView), false, BindingMode.TwoWay);

        private readonly OrderManager OrderManager;
        private readonly HtmlManager HtmlService;

        public bool? Shown
        {
            get => (bool?)GetValue(ShownProperty);
            set => SetValue(ShownProperty, value);
        }

        public HistoryHeaderView()
        {
            InitializeComponent();

            OrderManager = TinyIoCContainer.Current.Resolve<OrderManager>();
            HtmlService = TinyIoCContainer.Current.Resolve<HtmlManager>();
            BindingContext = OrderManager.Info;
        }

        private async void OpenPaymentInfo(object sender, EventArgs e)
        {
            var page = await HtmlService.GetPageAsync(GeneralResources.PayInfoTitle, urls => urls.Payment);

            await Navigation.PushAsync(page);
        }

        private async void LogOut(object sender, EventArgs e)
        {
            await OrderManager.Logout();
            await Navigation.PopToRootAsync();
        }
    }
}