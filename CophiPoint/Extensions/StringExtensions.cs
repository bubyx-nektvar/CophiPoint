using CophiPoint.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CophiPoint.Extensions
{
    public static class StringExtensions
    {
        public static string GetPriceString(this decimal price)
        {
            return price.ToString("N") + " Kč";
        }

        public static string GetSizeString(this decimal size, Unit unit)
        {
            return size + unit.ToAbbrevation();
        }
    }
}
