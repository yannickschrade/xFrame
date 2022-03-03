using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using xFrame.WPF.Theming.Themes;

namespace xFrame.WPF.Theming
{
    public class ThemeManager
    {

        #region Dependency properties

        public static readonly DependencyProperty ThemeTypeProperty = DependencyProperty.RegisterAttached(
            "ThemeType", typeof(ThemeType), typeof(ThemeManager), new PropertyMetadata(ThemeType.SystemDefault));

        public static void SetThemeType(object element, ThemeType themeType)
        {
            switch (element)
            {
                case FrameworkElement frameworkElement:
                    frameworkElement.SetValue(ThemeTypeProperty, themeType);
                    ChangeTheme(frameworkElement, themeType);
                    break;
                case ResourceDictionary resourceDictionary:
                    resourceDictionary[ThemeTypeResourceKey] = themeType;
                    ChangeTheme(themeType, resourceDictionary);
                    break;
                default:
                    throw new NotSupportedException("element is not Supported for theming");
            }

        }

        public static ThemeType GetThemeType(object element)
        {
            return element switch
            {
                FrameworkElement frameworkElement => (ThemeType)frameworkElement.GetValue(ThemeTypeProperty),
                ResourceDictionary resourceDictionary => (ThemeType)resourceDictionary[ThemeTypeResourceKey],
                _ => throw new NotSupportedException("element is not supported for themeing"),
            };
        }

        public static readonly DependencyProperty CustomThemeProperty = DependencyProperty.RegisterAttached(
            "CustomTheme", typeof(string), typeof(ThemeManager), new PropertyMetadata(default));

        public static void SetCustomTheme(object element, string themeName)
        {
            switch (element)
            {
                case FrameworkElement frameworkElement:
                    frameworkElement.SetValue(CustomThemeProperty, themeName);
                    break;
                case ResourceDictionary resourceDictionary:
                    resourceDictionary[CustomThemeResourceKey] = themeName;
                    break;
                default:
                    throw new NotSupportedException("element is not supported for themeing");
            }
        }

        public static string GetCustomTheme(object element)
        {
            return element switch
            {
                FrameworkElement frameworkElement => (string)frameworkElement.GetValue(CustomThemeProperty),
                ResourceDictionary resourceDictionary => (string)resourceDictionary[CustomThemeResourceKey],
                _ => throw new NotSupportedException("element is not supported for themeing"),
            };

        }

        #endregion

        private const string CustomThemeResourceKey = "customTheme";
        private const string ThemeTypeResourceKey = "themeType";

        private Theme _activeTheme;
        private readonly static Dictionary<ThemeType, Theme> _resources = new Dictionary<ThemeType, Theme>();
        private readonly static Dictionary<string, Theme> _customThemes;

        public ThemeType ThemeType { get; private set; }

        private static ThemeManager _current;
        public static ThemeManager Current => _current ??= new ThemeManager();

        static ThemeManager()
        {
            _resources[ThemeType.Dark] = new DarkTheme();
        }

        public static void ChangeTheme(FrameworkElement element, ThemeType theme)
        {
            var res = element.Resources;
            res[CustomThemeResourceKey] = GetCustomTheme(element);
            ChangeTheme(theme, res);
        }

        public static void AddTheme(Theme theme, string key = null)
        {
            if (key == null)
                key = theme.ThemeName;
        }

        public static void RegisterTheme(Theme theme)
        {
            switch (theme.ThemeType)
            {
                case ThemeType.SystemDefault:
                    _resources[ThemeType.SystemDefault] = theme;
                    break;
                case ThemeType.Dark:
                    _resources[ThemeType.Dark] = theme;
                    break;
                case ThemeType.Light:
                    _resources[ThemeType.Light] = theme;
                    break;
                case ThemeType.Custom:
                    _customThemes[theme.ThemeName] = theme;
                    break;
                default:
                    break;
            }
        }

        private static void ChangeTheme(ThemeType themeType, ResourceDictionary dictionary)
        {
            Theme theme;
            switch (themeType)
            {
                case ThemeType.SystemDefault:
                    theme = _resources.ContainsKey(ThemeType.SystemDefault)
                            ? _resources[ThemeType.SystemDefault]
                            : ThemeHelper.AppUsesLightTheme() ?
                                _resources[ThemeType.Light] :
                                _resources[ThemeType.Dark];
                    break;
                case ThemeType.Custom:
                    var themeName = (string)dictionary[CustomThemeResourceKey];
                    theme = _customThemes[themeName];
                    break;
                default:
                    throw new NotSupportedException($"themetype {themeType} is not supported");
            }

            dictionary.MergedDictionaries.Add(theme);
        }

    }
}
