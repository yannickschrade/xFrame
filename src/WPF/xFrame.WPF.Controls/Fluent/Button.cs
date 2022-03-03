using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace xFrame.WPF.Controls.Fluent
{
    public static class Button
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(Button),
            new PropertyMetadata(new CornerRadius(0)));

        public static CornerRadius GetCornerRadius(System.Windows.Controls.Button element)
            => (CornerRadius)element.GetValue(CornerRadiusProperty);

        public static void SetCornerRadius(System.Windows.Controls.Button element, CornerRadius cornerRadius)
            => element.SetValue(CornerRadiusProperty, cornerRadius);



        public static readonly DependencyProperty HilightBrushPropery = DependencyProperty.RegisterAttached(
            "HilightBrush", typeof(Brush), typeof(Button));

        public static Brush GetHilightBrush(System.Windows.Controls.Button element)
            => (Brush)element.GetValue(HilightBrushPropery);

        public static void SetHilightBrush(System.Windows.Controls.Button element, Brush brush)
            => element.SetValue(HilightBrushPropery, brush);



        public static readonly DependencyProperty HilightedElementProperty = DependencyProperty.RegisterAttached(
            "HilightedElement", typeof(ElementType), typeof(Button));

        public static ElementType GetHilightedElement(System.Windows.Controls.Button element)
            => (ElementType)element.GetValue(HilightedElementProperty);

        public static void SetHilightedElement(System.Windows.Controls.Button element, ElementType type)
            => element.SetValue(HilightedElementProperty, type);
    }
}
