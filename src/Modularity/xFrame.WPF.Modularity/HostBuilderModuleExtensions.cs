using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using xFrame.Modularity.Abstraction;
using xFrame.WPF.Modularity.@internal;
using xFrame.WPF.Modularity.Internal;

namespace xFrame.WPF.Modularity
{
    public static class HostBuilderModuleExtensions
    {
        public static IHostBuilder ConfigureModules(this IHostBuilder builder,
            Action<IModuleCollection> configuredelegate)
        {
            var collection = GetModuleCollection(builder);
            configuredelegate(collection);
            

            builder.ConfigureServices((context, services) =>
            {
                if(!builder.Properties.TryGetValue("ModuleLoaders", out var moduleLoaders))
                {
                    moduleLoaders = new ModuleLoaderCollection();
                    builder.Properties.Add("ModuleLoaders", moduleLoaders);
                    ((IModuleLoaderCollection)moduleLoaders).TryAdd(typeof(IModule), new DefaultModuleLoader());
                    services.AddSingleton<IModuleManager, ModuleManager>();
                    services.AddSingleton((IModuleLoaderCollection)moduleLoaders);
                }

                var loaders = (IModuleLoaderCollection) moduleLoaders;
                
                foreach (var moduleType in GetModuleTypes(collection))
                {
                    var loader = loaders.GetLoaderFor(moduleType);
                    var module = loader.CreateModule(context,services,moduleType);
                    services.AddSingleton(module);
                }
            });

            return builder;
        }

        public static IHostBuilder ConfigureModuleLoader(this IHostBuilder builder,
            Action<IModuleLoaderCollection> configureDelegate)
        {
            builder.ConfigureServices((context, services) =>
            {
                if (!builder.Properties.TryGetValue("ModuleLoaders", out var moduleLoaders))
                {
                    moduleLoaders = new ModuleLoaderCollection();
                    builder.Properties.Add("ModuleLoaders", moduleLoaders);
                    ((IModuleLoaderCollection)moduleLoaders).TryAdd(typeof(IModule), new DefaultModuleLoader());
                    services.AddSingleton<IModuleManager, ModuleManager>();
                    services.AddSingleton((IModuleLoaderCollection)moduleLoaders);
                }
                
                configureDelegate((IModuleLoaderCollection) moduleLoaders);
            });
            
            return builder;
        }

        private static IModuleCollection GetModuleCollection(IHostBuilder builder)
        {
            if (!builder.Properties.ContainsKey(nameof(ModuleCollection)))
            {
                builder.Properties[nameof(ModuleCollection)] = new ModuleCollection();
            }

            return (IModuleCollection) builder.Properties[nameof(ModuleCollection)];
        }

        private static IEnumerable<Type> GetModuleTypes(IModuleCollection modules)
        {
           
            foreach (var module in modules)
            {
                if (!File.Exists(module.Location))
                    continue;

                if (!module.LoadModule)
                    continue;

                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(module.Location);
                if (assembly == null)
                    continue;

                if (assembly == null)
                    continue;

                var moduleTypes = assembly.GetTypes()
                    .Where(t => typeof(IModule).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
                
                foreach (var m in moduleTypes)
                {
                    yield return m;
                }
            }
        }
    }
}