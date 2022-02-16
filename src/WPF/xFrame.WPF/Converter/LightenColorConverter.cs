using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Media;
using xFrame.WPF.Extensions;

namespace xFrame.WPF.Converter
{
    public class LightenColorConverter : BaseValueConverter<LightenColorConverter, SolidColorBrush>
    {
        public override object Convert(SolidColorBrush value, Type targetType, object parameter, CultureInfo culture)
        {
            var col = value.Color;
            if (parameter is uint amount)
                col = col.Lighten(amount);
            else
                throw new ArgumentException($"parameter has to be an {typeof(uint)}");

            return new SolidColorBrush(col);
        }

        public override object ConvertBack(SolidColorBrush value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
