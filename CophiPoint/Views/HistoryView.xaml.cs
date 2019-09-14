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
    public partial class HistoryView : ContentView
    {


        public static readonly BindableProperty ShownProperty = BindableProperty.Create(nameof(Shown), typeof(bool?), typeof(HistoryView), false, BindingMode.TwoWay);

        public bool? Shown
        {
            get => (bool?)GetValue(ShownProperty);
            set => SetValue(ShownProperty, value);
        }

        public ObservableCollection<PurchasedItemViewModel> History { get; set; }

        public HistoryView()
        {
            var service = new RestService();

            InitializeComponent();
            History = new ObservableCollection<PurchasedItemViewModel>(service.GetPurchases().Select(x => new PurchasedItemViewModel(x)));

            BindingContext = History;
        }
    }
}