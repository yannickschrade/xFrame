using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using xFrame.Modularity.Abstraction;

namespace xFrame.WPF.Modularity
{
    public static class HostBuilderModuleExtensions
    {
        public static IHostBuilder ConfigureModules(this IHostBuilder builder, Action<IModuleCollection> configuredelegate)
        {
            var collection = GetModuleCollection(builder);
            configuredelegate(collection);

            foreach (var module in collection)
            {

            }

            return builder;
        }


        private static IModuleCollection GetModuleCollection(IHostBuilder builder)
        {
            if (!builder.Properties.ContainsKey(nameof(ModuleCollection)))
            {
                builder.Properties[nameof(ModuleCollection)] = new ModuleCollection();
            }

            return (IModuleCollection)builder.Properties[nameof(ModuleCollection)];
        }

        private static IEnumerable<Assembly> LoadAssembly(IModuleCollection modules)
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



            }
        }
    }
}