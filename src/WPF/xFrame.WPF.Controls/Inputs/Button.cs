using System.Windows;
using System.Windows.Media;

namespace xFrame.WPF.Controls.Inputs
{
    public class Button : System.Windows.Controls.Button
    {

        public Brush HilightColor
        {
            get { return (Brush)GetValue(HilightColorProperty); }
            set { SetValue(HilightColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HilightColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HilightColorProperty =
            DependencyProperty.Register(nameof(HilightColor), typeof(Brush), typeof(Button), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(190, 215, 255))));


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(Button), new PropertyMetadata(new CornerRadius(0)));


        static Button()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
        }
    }
}
