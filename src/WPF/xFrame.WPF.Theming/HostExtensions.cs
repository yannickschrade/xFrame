using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using xFrame.WPF.Hosting;
using xFrame.WPF.Theming.Themes;

namespace xFrame.WPF.Theming
{
    public static class HostExtensions
    {
        public static XFrameHostBuilder UseColorTheme(this XFrameHostBuilder builder, ThemeType themeType)
        {
            builder.ConfigureResources(r => ThemeManager.ChangeTheme(themeType, r));
            return builder;
        }
    }
}
