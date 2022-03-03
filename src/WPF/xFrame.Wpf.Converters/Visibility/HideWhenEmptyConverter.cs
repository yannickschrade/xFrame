using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace xFrame.WPF.Converters.Visibility
{
    public class HideWhenEmptyConverter : BaseValueConverter<HideWhenEmptyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var empty = false;
            var invert = false;
            if (parameter != null)
                bool.TryParse(parameter.ToString(), out invert);
            switch (value)
            {
                case null:
                case string s when string.IsNullOrWhiteSpace(s):
                case ICollection { Count: 0 }:
                case Array { Length: 0 }:
                case IEnumerable e when !e.GetEnumerator().MoveNext():
                    empty = true;
                    break;
            }
            if (invert == true)
                empty = !empty;

            return empty ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
