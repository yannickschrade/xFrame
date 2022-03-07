
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using xFrame.Core.Modularity;

namespace xFrame.WPF
{
    public interface IAppBuilder
    {
        IServiceCollection Services { get; }
        IModuleCollection Modules { get; }

        IAppBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate);
        IAppBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate);
        IAppBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate);
        IAppBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory);
        IAppBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory);
        IHost Build();
    }
}