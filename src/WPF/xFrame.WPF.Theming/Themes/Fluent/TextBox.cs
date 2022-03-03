using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace xFrame.WPF.Theming.Themes.Fluent
{
    public static class TextBox
    {
        public static readonly DependencyProperty LabelProperty = DependencyProperty.RegisterAttached(
            "Label", typeof(string), typeof(TextBox), 
            new FrameworkPropertyMetadata(null));

        public static void SetLabel(TextBoxBase element, string label)
            => element.SetValue(LabelProperty, label);

        public static string GetLabel(TextBoxBase element)
            => (string)element.GetValue(LabelProperty);


        public static readonly DependencyProperty PlaceHolderTextProperty = DependencyProperty.RegisterAttached(
            "PlaceHolderText", typeof(string), typeof(TextBox), 
            new FrameworkPropertyMetadata(null));

        public static void SetPlaceHolderText(TextBoxBase element, string label)
            => element.SetValue(PlaceHolderTextProperty, label);

        public static string GetPlaceHolderText(TextBoxBase element)
            => (string)element.GetValue(PlaceHolderTextProperty);


        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
           "CornerRadius", typeof(CornerRadius), typeof(TextBox),
           new PropertyMetadata(new CornerRadius(0)));

        public static CornerRadius GetCornerRadius(TextBoxBase element)
            => (CornerRadius)element.GetValue(CornerRadiusProperty);

        public static void SetCornerRadius(TextBoxBase element, CornerRadius cornerRadius)
            => element.SetValue(CornerRadiusProperty, cornerRadius);
    }
}
