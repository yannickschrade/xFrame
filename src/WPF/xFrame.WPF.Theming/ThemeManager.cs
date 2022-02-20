using System;
using System.Collections.Generic;
using System.Windows;
using xFrame.WPF.Theming.Themes;

namespace xFrame.WPF.Theming
{
    public class ThemeManager
    {

        #region Dependency properties

        public static readonly DependencyProperty ThemeProperty = DependencyProperty.RegisterAttached(
            "Theme", typeof(string), typeof(ThemeManager), new PropertyMetadata(null, OnThemeChanged));

        public static void SetTheme(object element, string name)
        {
            switch (element)
            {
                case FrameworkElement frameworkElement:
                    frameworkElement.SetValue(ThemeProperty, name);
                    break;
                case Application app:
                    Current.ChangeAppTheme(app, name);
                    break;
                case ResourceDictionary resourceDictionary:
                    Current.AddThemeToDictionary(resourceDictionary, name);
                    break;
                default:
                    break;
            }

        }

        public static string GetTheme(FrameworkElement element)
            => (string)element.GetValue(ThemeProperty);

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChangeTheme((FrameworkElement)d, (string)e.NewValue);
        }

        #endregion


        private Theme _activeTheme;
        private Application _app;
        private bool _defaultsAdded;
        private Dictionary<string, Theme> _themes = new Dictionary<string, Theme>();

        private static ThemeManager _current;
        public static ThemeManager Current => _current ??= new ThemeManager();

        private ThemeManager()
        {
            _themes.Add("Dark", new DarkTheme());
        }

        public Theme this[string name] => _themes[name];


        public static void ChangeTheme(FrameworkElement element, string themeName)
        {
            if (!Current._themes.ContainsKey(themeName))
                throw new KeyNotFoundException($"the theme");

            element.Resources = Current._themes[themeName];
        }

        public void ChangeAppTheme(Application app, string themeName)
        {
            
            if (themeName == "SystemDefault")
                themeName = ThemeHelper.AppUsesLightTheme() ? "Light" : "Dark";

            if (!_themes.ContainsKey(themeName))
                throw new KeyNotFoundException($"theme with name: {themeName} not found");

            if (_activeTheme != null)
                app.Resources.MergedDictionaries.Remove(_activeTheme);

            _activeTheme = _themes[themeName];
            app.Resources.MergedDictionaries.Add(_themes[themeName]);
            app.Resources["ActiveTheme"] = themeName;
        }


        public static void AddTheme(Theme theme, string key = null)
        {
            if (key == null)
                key = theme.ThemeName;
        }

        public static void AddOrReplaceTheme(Theme theme, string key)
        {
            Current._themes[key] = theme;
            Current._themes.Add(key, theme);
            if ((string)Current._app.Resources["ActiveTheme"] == key)
                Current.ChangeAppTheme(Current._app, key);
        }

        public void AddThemeToDictionary(ResourceDictionary dictionary, string themeName)
        {
            if(themeName == "SystemDefault")
                themeName = ThemeHelper.AppUsesLightTheme() ? "Light" : "Dark";

            if (!_themes.ContainsKey(themeName))
                throw new KeyNotFoundException($"theme with name: {themeName} not found");

            dictionary.MergedDictionaries.Add(_themes[themeName]);
        }

        public static ThemeManager Initialize(Application app)
        {
            Current.ChangeAppTheme(app, "SystemDefault");
            Current._app = app;
            return Current;
        }
    }
}
