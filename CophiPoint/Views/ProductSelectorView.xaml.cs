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
using TinyIoC;
using CophiPoint.Services.Implementation;

namespace CophiPoint.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductSelectorView : ContentView
    {
        private readonly ProductManager _productManager;

        public ProductSelectorView()
        {
            InitializeComponent();

            _productManager = TinyIoCContainer.Current.Resolve<ProductManager>();
            BindingContext = _productManager.Products;
            ProductCarousel.SelectedItem = _productManager.Favorite;
        }

        private void OpenProductSearch(object sender, EventArgs e)
        {
            var page = TinyIoCContainer.Current.Resolve<ProductSearchPage>();
            page.SetBinding(new Binding(CarouselViewLayout.SelectedItemProperty.PropertyName, source: ProductCarousel, mode: BindingMode.OneWayToSource));
            Navigation.PushAsync(page, true);
        }
    }
}