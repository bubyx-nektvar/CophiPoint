using CophiPoint.Models;
using CophiPoint.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CophiPoint
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public IList<Product> Products { get; private set; }
        public MainPage()
        {
            InitializeComponent();
            Products = new RestService().GetProducts();
            BindingContext = this;
        }
    }
}
