using FFImageLoading.Svg.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Models
{
    public class Product
    {
        public string Name { get; set; }
        public Unit Unit { get; set; }
        public decimal PricePerUnit { get; set; }
        
        /// <summary>
        /// Size in units
        /// </summary>
        public decimal DefaultSize { get; set; }
        public decimal Price { get; set; }
        public Uri ImageUrl { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
