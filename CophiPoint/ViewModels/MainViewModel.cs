using CophiPoint.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CophiPoint.ViewModels
{
    public class MainViewModel:AbstractViewModel
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<PurchasedItem> History { get; set; }

        public string User { get; set; }

        public decimal Balance { get; set; }
    }
}
