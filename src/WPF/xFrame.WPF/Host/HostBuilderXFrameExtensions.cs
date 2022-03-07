using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using xFrame.Core.MVVM;
using xFrame.Core.ViewInjection;
using xFrame.WPF.Host;
using xFrame.WPF.Internal;
using xFrame.WPF.ViewInjection;

namespace xFrame.WPF.Extensions
{
    public static class HostBuilderXFrameExtensions
    {

        private static bool TryRetrieveXFrameBuilder(this IDictionary<object, object> properties, out XFrameContext context)
        {
            if (properties.TryGetValue(nameof(XFrameContext), out var xFrameBuilderAsObject))
            {
                context = (XFrameContext)xFrameBuilderAsObject;
                return true;

            }
            context = new XFrameContext();
            properties[nameof(XFrameContext)] = context;
            return false;
        }
        public static IHostBuilder ConfigureXFrame(this IHostBuilder builder, Action<IXFrameBuilder> configureDelegate)
        {
            builder.ConfigureServices((hostContext, services) =>
            {
                var xFrameBuilder = new XFrameBuilder();
                configureDelegate?.Invoke(xFrameBuilder);
                if (!TryRetrieveXFrameBuilder(builder.Properties, out var xFrameContext))
                {
                    services.AddSingleton(xFrameContext);
                    services.AddSingleton<IXFrameContext>(s => s.GetService<XFrameContext>());
                    services.AddHostedService<XFrameHostedService>();
                    RegisterDefaultServices(services);
                }

                if (xFrameBuilder.ShellType == null)
                    throw new InvalidOperationException("shell not configured");

                xFrameContext.ShellViewModelType = xFrameBuilder.ShellType;
                xFrameContext.ShutdownMode = xFrameBuilder.ShutdownMode;
            });


            return builder;
        }

        public static IHostBuilder UseXFrameLifetime(this IHostBuilder builder, ShutdownMode shutdownMode = ShutdownMode.OnLastWindowClose)
        {
            builder.ConfigureServices(s =>
            {
                if (!TryRetrieveXFrameBuilder(builder.Properties, out var xFrameContext))
                {
                    throw new NotSupportedException("Please configure XFrame first!");
                }

                xFrameContext.ShutdownMode = shutdownMode;
                xFrameContext.IsLifetimeLinked = true;
            });

            return builder;
        }

        public static void UseShellViewModel<T>(this IXFrameBuilder builder)
            where T : class, IViewModel
        {
            builder.ShellType = typeof(T);
        }


        private static void RegisterDefaultServices(IServiceCollection services)
        {
            services.AddSingleton<IViewAdapterCollection, ViewAdapterCollection>();
            services.AddSingleton<IViewInjectionService, ViewInjectionService>();
            services.AddSingleton<ViewProvider>();
            services.AddSingleton<IViewProvider>(s => s.GetService<ViewProvider>());
            services.AddSingleton<IViewRegistration>(s => s.GetService<ViewProvider>());
        }


    }
}
