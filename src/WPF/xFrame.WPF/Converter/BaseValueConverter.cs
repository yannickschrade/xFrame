using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace xFrame.WPF.Converter
{
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : BaseValueConverter<T>, new()
    {

        private static readonly T _converter;

        static BaseValueConverter()
        {
            _converter = new T();
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter;
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);


        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}