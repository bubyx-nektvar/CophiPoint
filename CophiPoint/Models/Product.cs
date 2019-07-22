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

        public int DefaultSizeIndex { get; set; }

        public Uri ImageUrl { get; set; }

        public Size[] Sizes { get; set; }

        public Product() { }

        protected Product(Product product)
        {
            Name = product.Name;
            Unit = product.Unit;
            DefaultSizeIndex = product.DefaultSizeIndex;
            ImageUrl = product.ImageUrl;
            Sizes = product.Sizes;
        }

        public class Size
        {
            public decimal UnitsCount { get; set; }
            public decimal Price { get; set; }
            public decimal PricePerUnit => Price / UnitsCount;
        }
    }
}
