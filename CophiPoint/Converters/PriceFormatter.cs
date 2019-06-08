using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Converters
{
    public class PriceFormatter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal v = (decimal)value;
            return v.ToString("N")+" Kč";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = ((string)value);
            return decimal.Parse(s.Substring(0, s.Length - 3));
        }
    }
}
