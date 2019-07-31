using CophiPoint.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CophiPoint.ViewModels
{
    public class MainViewModel: ReactiveObject
    {
        public ObservableCollection<ProductViewModel> Products { get; set; }
        public ObservableCollection<PurchasedItemViewModel> History { get; set; }

        public string User { get; set; }

        public decimal Balance { get; set; }

        [Reactive]
        public bool? HistoryShown { get; set; }
    }
}
