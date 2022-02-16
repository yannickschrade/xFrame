using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace xFrame.WPF.Extensions
{
    public static class ColorExtensions
    {

        /// <summary>
        /// Darkens the color 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="percentage">Range from 0 to 100%</param>
        /// <returns></returns>
        public static Color Darken(this Color color, uint percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentException("value has to be in range of 0 - 100%", nameof(percentage));
            }

            var value = percentage / 100f;

            float red = color.R;
            float green = color.G;
            float blue = color.B;

            value = 1 + value;
            red *= value;
            green *= value;
            blue *= value;


            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }

        /// <summary>
        /// Lightens the color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="percentage">Range from 0 to 100%</param>
        /// <returns></returns>
        public static Color Lighten(this Color color, uint percentage)
        {
            if (percentage < 0 || percentage > 100)
            {
                throw new ArgumentException("value has to be in range of 0 - 100%", nameof(percentage));
            }

            var value = percentage / 100f;

            float red = color.R;
            float green = color.G;
            float blue = color.B;

            red = (255 - red) * value + red;
            green = (255 - green) * value + green;
            blue = (255 - blue) * value + blue;


            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }

        public static SolidColorBrush Darken(this SolidColorBrush brush, uint percentage)
        {
            var col = brush.Color;
            return new SolidColorBrush(col.Darken(percentage));
        }

        public static SolidColorBrush Lighten(this SolidColorBrush brush, uint percentage)
        {
            var col = brush.Color;
            return new SolidColorBrush(col.Lighten(percentage));
        }

    }
}
