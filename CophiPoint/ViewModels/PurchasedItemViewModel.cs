using CophiPoint.Extensions;
using CophiPoint.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.ViewModels
{
    public class PurchasedItemViewModel
    {
        private readonly PurchasedItem _item;

        public DateTime Date => _item.Date;

        public string ProductName => _item.ProductName;

        public string PriceText => _item.TotalPrice.GetPriceString();

        public PurchasedItemViewModel(PurchasedItem item)
        {
            _item = item;
        }
    }
}
