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

        public Uri ImageUrl { get; set; }

        public Size[] Sizes { get; set; } = new Size[0];
    }
}
