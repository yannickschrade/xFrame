using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Windows;
using xFrame.Core.MVVM;

namespace xFrame.WPF.Hosting
{
    public static class XFrameHostBuilderExtensions
    {
        public static XFrameHostBuilder UseApp<TApp>(this XFrameHostBuilder builder)
            where TApp : XFrameApp
        {

            builder.Services.TryAddSingleton<XFrameApp,TApp>();
            return builder;
        }

        public static XFrameHostBuilder UseApp<TApp>(this XFrameHostBuilder builder, Func<IServiceProvider, TApp> factory)
            where TApp : XFrameApp
        {
            builder.Services.TryAddSingleton<XFrameApp>(factory);
            return builder;
        }


        public static XFrameHostBuilder AddResourceDictionary(this XFrameHostBuilder builder, ResourceDictionary resourceDictionary)
        {
            builder.ConfigureResources(r => r.MergedDictionaries.Add(resourceDictionary));
            return builder;
        }

        public static XFrameHostBuilder AddResourceDictionary(this XFrameHostBuilder builder, string pathToDictionary)
        {
            builder.ConfigureResources(r =>
            {
                var dic = new ResourceDictionary() { Source = new Uri("pack://application:,,,/" + pathToDictionary)};
                r.MergedDictionaries.Add(dic);
            });
            return builder;
        }

        public static XFrameHostBuilder UseSplashScreen<TViewModel>(this XFrameHostBuilder builder)
            where TViewModel : class, ISplashScreenViewModel
        {
            builder.Services.AddSingleton<ISplashScreenViewModel, TViewModel>();
            return builder;
        }
    }
}
