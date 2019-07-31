using CophiPoint.Extensions;
using CophiPoint.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.ViewModels
{
    public class SizeViewModel : ReactiveObject
    {
        private readonly Size _size;

        public string SizeText {get;set;}
        public decimal Price { get; set; }
        public string PricePerUnitText { get; set; }
        public string Image { get; set; }

        public SizeViewModel(Size size, Unit unit)
        {
            _size = size;
            SizeText = size.UnitsCount + unit.ToAbbrevation();
            Price = size.Price;
            var pricePerUnit = size.Price / size.UnitsCount;
            PricePerUnitText = pricePerUnit.GetPriceString() + "/" + unit.ToAbbrevation();
            Image = unit.ImageSource();
        }
    }
}
