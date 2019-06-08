using CophiPoint.Models;
using CophiPoint.Services;
using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ReactiveUI;
using DK.SlidingPanel.Interface;

namespace CophiPoint
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var service = new RestService();
            BindingContext = new MainViewModel()
            {
                Products = new ObservableCollection<Product>(service.GetProducts()),
                Balance = -123123.21m,
                User = "filip.havel@mojeaplikace.com",
                History = new ObservableCollection<PurchasedItem>(service.GetPurchases())
            };
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            SlidingPanelConfig config = new SlidingPanelConfig();

            StackLayout titleStackLayout = new StackLayout();
            titleStackLayout.Children.Add(new Label { Text = "Test Title x" });
            config.TitleBackgroundColor = Color.Green;

            slidingPanel.ApplyConfig(config);

        }
    }
}
