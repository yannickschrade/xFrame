using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using xFrame.Core.ExtensionMethodes;
using xFrame.Core.ViewInjection;
using xFrame.WPF.Hosting.Internal;
using xFrame.WPF.ViewInjection;

namespace xFrame.WPF.Hosting
{
    public sealed class XFrameHostBuilder
    {
        private readonly IHostBuilder _hostBuilder;
        private Func<IServiceProvider> _serviceProviderFactory;
        private ILoggingBuilder _loggingBuilder;
        private readonly List<KeyValuePair<string, string>> _hostConfigurationValues;


        public IServiceCollection Services { get; } = new ServiceCollection();
        public ILoggingBuilder Logging => _loggingBuilder ??= InitializeLogging();

        public ConfigurationManager Configuration { get; } = new ConfigurationManager();

        public HostOptions HostOptions { get; } = new HostOptions();

        public HostConfiguration HostConfiguration { get; }

        public XFrameAppContext AppContext { get; } = new XFrameAppContext();

        private ILoggingBuilder InitializeLogging()
        {
            return new LoggingBuilder(Services);
        }

        internal XFrameHostBuilder(bool useDefaults)
        {
            _hostBuilder = Host.CreateDefaultBuilder();
            _hostConfigurationValues = new List<KeyValuePair<string, string>>(Configuration.AsEnumerable());

            var env = new HostingEnvironment()
            {
                ApplicationName = Configuration[HostDefaults.ApplicationKey],
                EnvironmentName = Configuration[HostDefaults.EnvironmentKey] ?? Environments.Production,
                ContentRootPath = HostingPathResolver.ResolvePath(Configuration[HostDefaults.ContentRootKey]),
            };
            
            var hostContext = new HostBuilderContext(_hostBuilder.Properties)
            {
                Configuration = Configuration,
                HostingEnvironment = env
            };

            HostConfiguration = new HostConfiguration(hostContext, Configuration, Services);

            if (useDefaults)
            {
                _hostBuilder = Host.CreateDefaultBuilder();
                RegisterDefaultServices();
            }

            Services.AddSingleton(AppContext);
            Services.AddHostedService<HostedAppService>();

        }

        public void ConfigureResources(Action<ResourceDictionary> resourceBuilder)
        {
            Services.AddSingleton(resourceBuilder);
        }

        public XFrameHost Build()
        {

            _hostBuilder.ConfigureHostConfiguration(builder =>
            {
                builder.AddInMemoryCollection(_hostConfigurationValues);
            });

            var chainedSource = new TrackingChainedConfigurationSource(Configuration);

            _hostBuilder.ConfigureAppConfiguration(builder =>
            {
                builder.Add(chainedSource);

                foreach (var (key, value) in ((IConfigurationBuilder)Configuration).Properties)
                {
                    builder.Properties[key] = value;
                }

            });

            _hostBuilder.ConfigureServices((context,services) =>
            {
                foreach (var service in Services)
                {
                    services.Add(service);
                }

                var hostBuilderProviders = ((IConfigurationRoot)context.Configuration).Providers;

                if (!hostBuilderProviders.Contains(chainedSource.BuiltProvider))
                {
                    ((IConfigurationBuilder)Configuration).Sources.Clear();
                }

                
                foreach (var provider in hostBuilderProviders)
                {
                    if (!ReferenceEquals(provider, chainedSource.BuiltProvider))
                    {
                        ((IConfigurationBuilder)Configuration).Add(new ConfigurationProviderSource(provider));
                    }
                }

                services.TryAddSingleton<Application, XFrameApp>();
            });

            _hostBuilder.ConfigureHostOptions((c,h) =>
            {
                h.ShutdownTimeout = HostOptions.ShutdownTimeout;
                h.BackgroundServiceExceptionBehavior = HostOptions.BackgroundServiceExceptionBehavior;                
            });

            HostConfiguration.RunDeferredCallbacks(_hostBuilder);

            var host = new XFrameHost(_hostBuilder.Build());

            host.Services.GetService<IEnumerable<IConfiguration>>();
            return host;
        }

        private void RegisterDefaultServices()
        {
            Services.AddSingleton<IViewAdapterCollection, ViewAdapterCollection>();
            Services.AddSingleton<IViewInjectionService, ViewInjectionService>();
            Services.AddSingleton<ViewProvider>();
            Services.AddSingleton<IViewProvider>(s => s.GetService<ViewProvider>());
            Services.AddSingleton<IViewRegistration>(s => s.GetService<ViewProvider>());
            Services.AddSingleton(_ => Application.Current.Dispatcher);
        }

        private sealed class LoggingBuilder : ILoggingBuilder
        {
            public IServiceCollection Services { get; }

            public LoggingBuilder(IServiceCollection services)
            {
                Services = services;
            }
        }


    }



}
