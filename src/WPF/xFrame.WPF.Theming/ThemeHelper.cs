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
        public static bool AppUsesLightTheme()
        {
            var registryValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", null);

            if (registryValue is null)
            {
                return true;
            }

            return Convert.ToBoolean(registryValue);
        }

        public static Color GetAccentColor()
        {
            var accentPaletteRegistryValue = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Accent", "AccentPalette", null);

            if (accentPaletteRegistryValue is null)
            {
                var colorizationColorRegistryValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "ColorizationColor", null);
                var colorizationColorTypedRegistryValue = (uint)(int)colorizationColorRegistryValue;
                var colorizationColor = Color.FromRgb((byte)(colorizationColorTypedRegistryValue >> 16),
                                                          (byte)(colorizationColorTypedRegistryValue >> 8),
                                                          (byte)colorizationColorTypedRegistryValue);

                return colorizationColor;
            }

            var bin = (byte[])accentPaletteRegistryValue;

            return Color.FromRgb(bin[0x0C], bin[0x0D], bin[0x0E]);

        }
    }
}
