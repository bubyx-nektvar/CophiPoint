using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Api.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public Unit Unit { get; set; }

        public Size[] Sizes { get; set; } = new Size[0];
    }
}
