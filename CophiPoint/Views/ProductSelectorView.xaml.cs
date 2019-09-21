using CophiPoint.Pages;
using CophiPoint.Services;
using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CophiPoint.Components;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductSelectorView : ContentView
    {

        public ObservableCollection<ProductViewModel> Products { get; private set; }

        public ProductSelectorView()
        {
            InitializeComponent();

            Products = ((App)Application.Current).ProductManager.Products;
            BindingContext = Products;
        }

        private void OpenProductSearch(object sender, EventArgs e)
        {
            var page = new ProductSearchPage(Products);
            page.SetBinding(new Binding(CarouselViewLayout.SelectedItemProperty.PropertyName, source: ProductCarousel, mode: BindingMode.OneWayToSource));
            Navigation.PushAsync(page, true);
        }
    }
}