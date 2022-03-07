using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using xFrame.WPF.Theming.Themes;

namespace xFrame.WPF.Theming
{
    public static class HostExtensions
    {
        public static IHostBuilder UseColorTheme(this IHostBuilder builder, ThemeType themeType)
        {
            if (builder.Properties.TryGetValue("XFrameContext", out var context))
                throw new InvalidOperationException("please configure xframe app first!");

            builder.ConfigureServices((context, services)
                => services.AddSingleton(
                    s => new Action<Application>(app => ThemeManager.ChangeTheme(themeType, app.Resources))));

            return builder;
        }
    }
}
