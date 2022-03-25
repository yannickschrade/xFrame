using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Windows;

namespace xFrame.WPF.Hosting
{
    public static class XFrameAppBuilderExtensions
    {
        public static XFrameHostBuilder UseApp<TApp>(this XFrameHostBuilder builder)
            where TApp : Application
        {

            builder.Services.TryAddSingleton<Application,TApp>();
            return builder;
        }

        public static XFrameHostBuilder UseApp<TApp>(this XFrameHostBuilder builder, Func<IServiceProvider, TApp> factory)
            where TApp : Application
        {
            builder.Services.TryAddSingleton<Application>(factory);
            return builder;
        }


        public static XFrameHostBuilder AddResourceDictionary(this XFrameHostBuilder builder, ResourceDictionary resourceDictionary)
        {
            builder.ConfigureResources(r => r.MergedDictionaries.Add(resourceDictionary));
            return builder;
        }
    }
}
