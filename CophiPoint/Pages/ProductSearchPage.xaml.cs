using CophiPoint.Services;
using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductSearchPage : ContentPage
    {

        public ObservableCollection<ProductViewModel> Products { get; }
        public ProductSearchPage(ObservableCollection<ProductViewModel> products)
        {
            InitializeComponent();

            BindingContext = products;
        }

        public void SetBinding(BindingBase binding) => ProductList.SetBinding(ListView.SelectedItemProperty, binding);

        private void ProductSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Navigation.PopAsync(true);
        }
    }
}