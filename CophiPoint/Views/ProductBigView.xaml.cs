using CophiPoint.Api;
using CophiPoint.Services;
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
        private readonly OrderManager _orderManager;
        private readonly ProductManager _productManager;

        public ProductViewModel Product
        {
            get { return (ProductViewModel)GetValue(ProductProperty); }
            set { SetValue(ProductProperty, value); }
        }
        
        public ProductBigView()
        {
            _orderManager = ((App)Application.Current).OrderManager;
            _productManager = ((App)Application.Current).ProductManager;

            InitializeComponent();
            BindingContext = Product;
        }

        private async void Order(object sender, EventArgs e)
        {
            var result = await App.Current.MainPage.DisplayAlert(
                GeneralResources.OrderConfirmAlertTitle, 
                string.Format(GeneralResources.OrderConfirmAlertMsg,Product.Name,Product.SelectedSize.SizeText),
                GeneralResources.AlertAdd,
                GeneralResources.AlertCancel);
            if (result)
            {
                await _orderManager.AddItem(Product);
            }
        }

        private void SwitchFavorite(object sender, EventArgs e)
        {
            _productManager.SetFavorite(Product, !Product.Favorite);
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