using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Converters
{
    public class JoinConverterGroup : List<IValueConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<object> result = new List<object>();
            bool unionResult = ((string)parameter) == "group";
            object last = null;
            foreach(var converter in this)
            {
                var x = converter.Convert(value, targetType, parameter, culture);
                if (x != last)
                {
                    last = x;
                    result.Add(x);
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
