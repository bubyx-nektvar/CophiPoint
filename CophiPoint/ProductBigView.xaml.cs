﻿using CophiPoint.Models;
using System;
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
            //SelectSize = new Command<decimal>(execute: size => {
            //    Product.UseSize(size);
            //    SizesExpanded = false;
            //});
            InitializeComponent();
            BindingContext = Product;
        }

        async void ShowSizes(object sender, EventArgs args)
        {
            //SizesExpanded = true;
        }

        //public bool SizesExpanded { get; set; } = false;
        //public Command SelectSize { get; set; }
    }
}