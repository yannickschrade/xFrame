using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace xFrame.WPF.Theming.ExtensionMethodes
{
    public static class ResourceDictionaryExtensions
    {
        public static void AddThemeColor(this ResourceDictionary dict, Color? color, string key)
        {
            if(color == null)
                throw new ArgumentNullException(nameof(color));

            dict.Add(key, color);
            var solidBrushKey = key.Contains("Key") ? key.Insert(key.Length - 3, "Brush") : key + "Brush";
            dict.Add(solidBrushKey, new SolidColorBrush((Color)color));
        }

        public static void AddThemeColor(this ResourceDictionary dictionary, string color, string key)
        {
            var col = (Color)ColorConverter.ConvertFromString(color);
            dictionary.AddThemeColor(col, key);
        }
    }
}
