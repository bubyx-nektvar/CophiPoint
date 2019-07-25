using CophiPoint.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CophiPoint.Converters
{
    public class SizeInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var unit = (Unit)Enum.Parse(typeof(Unit), ((Label)parameter).Text);
            var items = (Product.Size[])value;
            return items.Select(x => new SizeDefinition(x, unit)).ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        public struct SizeDefinition
        {
            public decimal Size { get; set; }
            public Unit Unit { get; set; }
            public string ImageSource { get; set; }
            public string Text { get; private set; }

            public SizeDefinition(Product.Size size, Unit unit)
            {
                Size = size.UnitsCount;
                Unit = unit;
                ImageSource = unit.ImageSource();
                Text = $"{size.UnitsCount} {unit.ToAbbrevation()}";
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
