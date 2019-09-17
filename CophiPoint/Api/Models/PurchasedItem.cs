using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Api.Models
{
    public class PurchasedItem
    {
        public int ProductId { get; set; }
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
