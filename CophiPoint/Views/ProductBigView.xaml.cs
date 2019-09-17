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
            InitializeComponent();
            BindingContext = Product;
        }


        private void Order(object sender, EventArgs e)
        {

        }

        private void SwitchFavorite(object sender, EventArgs e)
        {
            Product.Favorite = !Product.Favorite;
        }

        private void SizeSelected(object sender, ItemTappedEventArgs e)
        {
            Product.SelectedSize = e.Item as SizeViewModel;
            Product.SelectSizeVisible = false;
        }
        
        private void ChooseSize(object sender, EventArgs e)
        {
            Product.SelectSizeVisible = true;

        }
    }
}