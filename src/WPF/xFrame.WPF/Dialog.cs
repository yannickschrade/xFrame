using System.Windows;

namespace xFrame.WPF
{
    public static class Dialog
    {
        public static readonly DependencyProperty StyleProperty = DependencyProperty.RegisterAttached(
            "Style", typeof(Style), typeof(Dialog), new PropertyMetadata(default));

        public static Style GetStyle(DependencyObject obj)
            => (Style)obj.GetValue(StyleProperty);

        public static void SetStyle(DependencyObject obj, Style style)
            => obj.SetValue(StyleProperty, style);
    }
}
