using CophiPoint.Services;
using CophiPoint.Services.Implementation;
using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryView : ContentView
    {


        public static readonly BindableProperty ShownProperty = BindableProperty.Create(nameof(Shown), typeof(bool), typeof(HistoryView), false, BindingMode.OneWay);

        public bool Shown
        {
            get => (bool)GetValue(ShownProperty);
            set => SetValue(ShownProperty, value);
        }

        public ObservableCollection<PurchasedItemViewModel> History { get; set; }

        public HistoryView()
        {
            InitializeComponent();
            History = TinyIoCContainer.Current.Resolve<OrderManager>().Items;

            BindingContext = History;
        }
    }
}