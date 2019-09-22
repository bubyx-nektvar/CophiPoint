using CophiPoint.Extensions;
using CophiPoint.Api.Models;
using FFImageLoading.Svg.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.ViewModels
{
    public class SizeViewModel
    {
        internal readonly Size _size;

        public string SizeText {get;set;}
        public decimal Price { get; set; }
        public decimal PricePerUnit { get; set; }
        public string PricePerUnitText { get; set; }
        public SvgImageSource Image { get; set; }

        public SizeViewModel(Size size, Unit unit, SvgImageSource unitImage)
        {
            _size = size;
            SizeText = size.UnitsCount + unit.ToAbbrevation();
            Price = size.TotalPrice;
            PricePerUnit = size.TotalPrice / size.UnitsCount;
            PricePerUnitText = PricePerUnit.GetPriceString() + "/" + unit.ToAbbrevation();
            Image = unitImage;
        }
    }
}
