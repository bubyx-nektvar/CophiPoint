using CophiPoint.Components;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Converters
{
    public class PanelShownConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var v = (PanelState)value;
            return v == PanelState.Shown;
            //has navigation bar
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var v = (bool)value;
            return v ? PanelState.Shown : PanelState.Hidden;
        }
    }
}
