using CophiPoint.Services;
using CophiPoint.Services.Implementation;
using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductSearchPage : ContentPage
    {
        private readonly ProductManager manager;

        public ObservableCollection<ProductViewModel> Products { get; }
        public ObservableCollection<ProductViewModel> Hidden { get; }

        private SemaphoreSlim _lock = new SemaphoreSlim(1);

        public ProductSearchPage(ProductManager productManager)
        {
            InitializeComponent();
            manager = productManager;
            Products = new ObservableCollection<ProductViewModel>(manager.Products);
            Hidden = new ObservableCollection<ProductViewModel>();
            BindingContext = Products;
        }

        public void SetBinding(BindingBase binding) => ProductList.SetBinding(ListView.SelectedItemProperty, binding);

        private async void ProductSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = e.NewTextValue.ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            await _lock.WaitAsync();
            var oldValue = e.OldTextValue ?? "";
            var newValue = e.NewTextValue ?? "";
            try
            {
                if (newValue.Length > oldValue.Length)
                {
                    foreach (var x in Products.Where(p => !search.All(x => p.Name.ToLowerInvariant().Contains(x))).ToList())
                    {
                        Hidden.Add(x);
                        Products.Remove(x);
                    }
                }
                else if (newValue.Length < oldValue.Length)
                {
                    foreach (var i in Hidden.Where(p => search.All(x => p.Name.ToLowerInvariant().Contains(x))).ToList())
                    {
                        Products.Add(i);
                        Hidden.Remove(i);
                    }
                }
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}