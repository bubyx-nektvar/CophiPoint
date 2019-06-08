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
            ViewModel = new MainViewModel()
            {
                Products = new ObservableCollection<Product>(service.GetProducts()),
                Balance = -1123.21m,
                User = "filip.havel@mojeaplikace.com",
                History = new ObservableCollection<PurchasedItem>(service.GetPurchases()),
                HistoryShown = false
            };
            BindingContext = ViewModel;

            HistoryPage.TranslationY = 2000;
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

        }
        private double TranslationYMinimized => Page.Height * 0.8;

        public MainViewModel ViewModel { get; }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            HistoryPage.TranslationY = TranslationYMinimized;
        }

        async void ShowHistory(object sender, System.EventArgs e)
        {
            await HistoryPage.TranslateTo(0, 0, 500, Easing.SinIn);
            ViewModel.HistoryShown = true;
        }

        async void HideHistory(object sender, System.EventArgs e)
        {
            await HistoryPage.TranslateTo(0, TranslationYMinimized, 500, Easing.SinIn);
            ViewModel.HistoryShown = false;
        }

    }
}
