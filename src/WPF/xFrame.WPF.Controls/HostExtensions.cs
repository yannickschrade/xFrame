using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace xFrame.WPF.Controls
{
    public static class HostExtensions
    {
        public static IHostBuilder UseFluentControls(this IHostBuilder builder)
        {
            if (builder.Properties.TryGetValue("XFrameContext", out var context))
                throw new InvalidOperationException("please configure xframe app first!");


            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton(s => new Action<Application>(app =>
                {
                    app.Resources.MergedDictionaries.Add(new FluentControls());
                }));
            });

            return builder;
        }
    }
}
