using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Api.Models
{
    public class PurchaseOrder
    {
        public int ProductId { get; set; }
        
        public Size Size { get; set; }
    }
}
