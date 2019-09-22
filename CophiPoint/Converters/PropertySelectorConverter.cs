using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Converters
{
    public class PropertySelectorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var propertyName = (string)parameter;
            var collection = (IEnumerable)value;
            var result = new List<object>();
            foreach(var item in collection)
            {
                var x = item.GetType().GetProperty(propertyName).GetValue(item);
                result.Add(x);
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
