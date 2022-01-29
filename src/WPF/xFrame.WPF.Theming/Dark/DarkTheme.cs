using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using xFrame.WPF.Theming.ExtensionMethodes;

namespace xFrame.WPF.Theming.Dark
{
    public partial class DarkTheme : Theme
    {
        public DarkTheme()
        {
            Name = Themes.Dark;

            AddColors();
            AddDefaults();
        }

        private void AddDefaults()
        {
            MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/xFrame.WPF.Theming;component/Default/Default.xaml") });
        }

        private void AddColors()
        {
            var accentColor = ThemeHelper.GetAccentColor();
            this.AddThemeColor(accentColor, ThemeColor.AccentColorKey);
            
            this.AddThemeColor("#202020", ThemeColor.BaseColorKey);
            this.AddThemeColor("#303030", ThemeColor.BaseColor1Key);
            this.AddThemeColor("#505050", ThemeColor.BaseColor2Key);
            this.AddThemeColor("#252525", ThemeColor.BaseColorDisabledKey);

            this.AddThemeColor("white", ThemeColor.ForegroundColorKey);
            this.AddThemeColor("#808080", ThemeColor.ForegroundDisabledColorKey);
        }
    }
}
