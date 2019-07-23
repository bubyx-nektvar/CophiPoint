using CophiPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryViewCell : ViewCell
    {
        public static readonly BindableProperty ItemProperty = BindableProperty.Create(nameof(Item), typeof(PurchasedItem), typeof(HistoryViewCell), new PurchasedItem());

        public PurchasedItem Item
        {
            get { return (PurchasedItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public HistoryViewCell()
        {
            InitializeComponent();
            BindingContext = Item;
        }
    }
}