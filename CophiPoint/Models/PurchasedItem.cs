﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Models
{
    public class PurchasedItem
    {
        public DateTime Date { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
