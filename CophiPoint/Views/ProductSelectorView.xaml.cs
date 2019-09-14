﻿using CophiPoint.Services;
using CophiPoint.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CophiPoint.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductSelectorView : ContentView
    {
        public ObservableCollection<ProductViewModel> Products { get; private set; }

        public ProductSelectorView()
        {

            var service = new RestService();
            InitializeComponent();

            Products = new ObservableCollection<ProductViewModel>(service.GetProducts().Select(x => new ProductViewModel(x)));
            BindingContext = Products;
        }

    }
}