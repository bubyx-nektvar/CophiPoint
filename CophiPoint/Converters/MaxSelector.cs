using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Converters
{
    public class MaxSelector : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = (IEnumerable)value;

            var e = collection.GetEnumerator();
            if (!e.MoveNext())
            {
                return null;
            }
            var max = e.Current;
            var comparer = Comparer.Default;
            foreach(var x in collection)
            {
                if(comparer.Compare(max, x)< 0)
                {
                    max = x;
                }
            }
            return max;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
