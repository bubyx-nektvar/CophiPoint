using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Api.Models
{
    public class AccountInfo
    {
        public string DataVersion { get; set; }

        public string Email { get; set; }

        public decimal Balance { get; set; }

        public List<PurchasedItem> Orders { get; set; } = new List<PurchasedItem>();
    }
}
