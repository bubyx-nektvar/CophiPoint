using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Converters
{
    public class MinSelector : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = (IEnumerable)value;

            var e = collection.GetEnumerator();
            if (!e.MoveNext())
            {
                return null;
            }
            var min = e.Current;
            var comparer = Comparer.Default;
            foreach(var x in collection)
            {
                if(comparer.Compare(min, x)> 0)
                {
                    min = x;
                }
            }
            return min;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
