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
                Products = new ObservableCollection<ProductViewModel>(service.GetProducts().Select(x=>new ProductViewModel(x))),
                Balance = -1123.21m,
                User = "filip.havel@mojeaplikace.com",
                History = new ObservableCollection<PurchasedItemViewModel>(service.GetPurchases().Select(x=>new PurchasedItemViewModel(x))),
                HistoryShown = false
            };
            BindingContext = ViewModel;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        public MainViewModel ViewModel { get; }

    }
}
