using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Converters
{
    public class PriceFormatter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is decimal)
            {
                return GetString((decimal)value);
            }else if(value is IEnumerable)
            {
                var result = new List<string>();
                foreach(var x in ((IEnumerable) value))
                {
                    if(x is decimal)
                    {
                        result.Add(GetString((decimal)x));
                    }
                }
                return result;
            }
            return null;
        }
        private string GetString(decimal v)
        {
            return v.ToString("N") + " Kč";
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = ((string)value);
            return decimal.Parse(s.Substring(0, s.Length - 3));
        }
    }
}
