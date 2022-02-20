using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace xFrame.WPF.Theming
{
    internal static class ThemeHelper
    {
        internal static bool AppUsesLightTheme()
        {
            var registryValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", null);

            if (registryValue is null)
            {
                return true;
            }

            return Convert.ToBoolean(registryValue);
        }

        internal static Color GetAccentColor()
        {
            var registryValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "AccentColor", null);

            Color accentColor;
            if (registryValue is null)
            {
                return Colors.Transparent;
            }

            var pp = (uint)(int)registryValue;
            if (pp > 0)
            {
                var bytes = BitConverter.GetBytes(pp);
                accentColor = Color.FromArgb(bytes[3], bytes[0], bytes[1], bytes[2]);
            }

            return accentColor;
        }
    }
}
