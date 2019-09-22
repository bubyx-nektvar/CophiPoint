using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Converters
{
    public class ChainConverterGroup : List<IValueConverter>, IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var args = ((string)parameter).Split('|');
            for(int i =0;i<Count;++i)
            {
                var converter = this[i];
                var arg = (args.Length > i) ? args[i] : null;
                value = converter.Convert(value, targetType, arg, culture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
