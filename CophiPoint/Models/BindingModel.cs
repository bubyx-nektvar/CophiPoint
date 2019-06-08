using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CophiPoint.Models
{
    internal class BindingModel
    {
        public ObservableCollection<Product> Products { get; set; }

        public string User { get; set; }

        public decimal Balance { get; set; }
    }
}
