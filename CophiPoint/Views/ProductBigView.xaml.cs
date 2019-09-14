using CophiPoint.Models;
using CophiPoint.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductBigView : ContentView
    {
        public static readonly BindableProperty ProductProperty = BindableProperty.Create(nameof(Product), typeof(ProductViewModel), typeof(ProductBigView),
            ProductViewModel.Empty
            );

        public ProductViewModel Product
        {
            get { return (ProductViewModel)GetValue(ProductProperty); }
            set { SetValue(ProductProperty, value); }
        }

        public ProductBigView()
        {
            //SelectSize = new Command<decimal>(execute: size => {
            //    Product.UseSize(size);
            //    SizesExpanded = false;
            //});
            InitializeComponent();
            BindingContext = Product;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Product.SelectedSize = e.Item as SizeViewModel;
            Product.SelectSizeVisible = false;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Product.SelectSizeVisible = true;
        }
    }
}