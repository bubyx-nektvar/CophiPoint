using CophiPoint.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint
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
    }
}