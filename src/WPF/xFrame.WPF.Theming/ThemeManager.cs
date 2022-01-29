using System;
using System.Collections.Generic;
using System.Windows;
using xFrame.WPF.Theming.Dark;

namespace xFrame.WPF.Theming
{
    public class ThemeManager
    {

        #region Dependency properties

        public static readonly DependencyProperty ThemeProperty = DependencyProperty.RegisterAttached(
            "Theme", typeof(string), typeof(ThemeManager), new PropertyMetadata(null, OnThemeChanged));

        public static void SetTheme(object element, string name)
        {
            if(element is FrameworkElement frameworkElement)
            {
                frameworkElement.SetValue(ThemeProperty, name);
                return;
            }

            if (element is ResourceDictionary resourceDictionary)
            {
                Current.AddThemeToDictionary(resourceDictionary, name);
                return;
            }

            throw new NotSupportedException($"elemnt must be an \"{typeof(FrameworkElement)}\" or \"{typeof(ResourceDictionary)}\"");
        }


        public static string GetTheme(FrameworkElement element)
            => (string)element.GetValue(ThemeProperty);

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Current.ChangeTheme((FrameworkElement)d, (string)e.NewValue);
        }

        public static DependencyProperty Themes = DependencyProperty.RegisterAttached(
            "Themes", typeof(ThemeCollection), typeof(ThemeManager));

        public static void SetThemes(Application app, ThemeCollection themeCollection)
        {
            foreach (var theme in themeCollection)
            {
                Current._themes.Add(theme.Name, theme);
            }
        }
        #endregion


        private Theme _activeTheme;
        private Dictionary<string, Theme> _themes = new Dictionary<string, Theme>();

        private static ThemeManager _current;
        public static ThemeManager Current => _current ??= new ThemeManager();

        private ThemeManager()
        {
            AddTheme(new DarkTheme());
        }

        public Theme this[string name] => _themes[name];


        public void ChangeTheme(FrameworkElement element, string themeName)
        {
            if (!_themes.ContainsKey(themeName))
                throw new KeyNotFoundException($"the theme");

            element.Resources = _themes[themeName];
        }

        public void ChangeAppTheme(Application app, string themeName)
        {
            if(themeName == Theming.Themes.SystemDefault)
                themeName = ThemeHelper.AppUsesLightTheme() ? Theming.Themes.Light : Theming.Themes.Dark;

            if (!_themes.ContainsKey(themeName))
                throw new KeyNotFoundException($"theme with name: {themeName} not found");

            if (_activeTheme != null)
                app.Resources.MergedDictionaries.Remove(_activeTheme);

            _activeTheme = _themes[themeName];
            app.Resources.MergedDictionaries.Add(_themes[themeName]);
        }

        public void AddTheme(Theme theme)
        {
            _themes.Add(theme.Name, theme);
        }

        public void AddThemeToDictionary(ResourceDictionary dictionary, string themeName)
        {
            if(!_themes.ContainsKey(themeName))
                throw new KeyNotFoundException($"theme with name: {themeName} not found");

            dictionary.MergedDictionaries.Add(_themes[themeName]);
        }
    }
}
