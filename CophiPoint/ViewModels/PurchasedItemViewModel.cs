using CophiPoint.Extensions;
using CophiPoint.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.ViewModels
{
    public class PurchasedItemViewModel : ReactiveObject
    {
        private readonly PurchasedItem _item;

        public DateTime Date => _item.Date;

        public string ProductName => _item.ProductName;

        public string PriceText => _item.Price.GetPriceString();

        public PurchasedItemViewModel(PurchasedItem item)
        {
            _item = item;
        }
    }
}
