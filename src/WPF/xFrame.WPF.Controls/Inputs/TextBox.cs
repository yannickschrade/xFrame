using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace xFrame.WPF.Controls.Inputs
{
    public class TextBox : System.Windows.Controls.TextBox
    {

        private TextBlock _headerTextBlock;
        private TextBlock _placeHolderText;
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(TextBox), new PropertyMetadata(new CornerRadius(5)));


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(TextBox), new PropertyMetadata(null, OnHeaderChanged));

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox._headerTextBlock == null)
                return;

            var text = e.NewValue as string;
            textBox._headerTextBlock.Visibility = string.IsNullOrWhiteSpace(text) ? Visibility.Collapsed : Visibility.Visible;
        }

        public string PlaceHolderText
        {
            get { return (string)GetValue(PlaceHolderTextProperty); }
            set { SetValue(PlaceHolderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderTextProperty =
            DependencyProperty.Register("PlaceHolderText", typeof(string), typeof(TextBox), new PropertyMetadata(null));


        static TextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
            TextProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(null, OnTextChanged));
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            if (textBox._placeHolderText == null)
                return;
            var text = e.NewValue as string;
            textBox._placeHolderText.Visibility = string.IsNullOrWhiteSpace(text) ? Visibility.Visible : Visibility.Hidden;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _headerTextBlock = (TextBlock)GetTemplateChild("PART_Header");
            _placeHolderText = (TextBlock)GetTemplateChild("PART_PlaceHolderText");
        }
    }
}
