using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Converters
{
    public class InverseBoolSuccessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var v = (bool?)value;
            return v.HasValue ? !v.Value : true;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
