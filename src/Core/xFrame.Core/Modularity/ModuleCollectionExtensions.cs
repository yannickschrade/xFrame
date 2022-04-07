using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace xFrame.Core.Modularity
{
    public static class ModuleCollectionExtensions
    {

        public static IModuleCollection AddModule(this IModuleCollection modules, Type moduleType)
        {
            if (!typeof(IServiceModule).IsAssignableFrom(moduleType) || !typeof(IModule).IsAssignableFrom(moduleType) || moduleType.IsAbstract || !moduleType.IsClass)
                throw new ArgumentException(nameof(moduleType) + " must be an non abstract class which implements the " + typeof(IServiceModule) + " or " + typeof(IModule) + " interface");

            modules.Add(new ModuleDescription(moduleType));
            return modules;
        }


        public static IModuleCollection AddModule<T>(this IModuleCollection modules)
            where T : class, IModule
        {
            return modules.AddModule(typeof(T));
        }

        public static IModuleCollection AddFromFolder(this IModuleCollection modules, string path)
        {
            var directories = Directory.GetDirectories(path);
            if (!directories.Any())
                throw new InvalidOperationException("Modules must be in a sub folder per module");

            foreach (var directory in directories)
            {
                string dirName = Path.GetFileName(directory);
                var file = Directory.GetFiles(directory, "*" + dirName + "*.dll").FirstOrDefault();
                if (string.IsNullOrWhiteSpace(file))
                    continue;

                var moduleTypes = Assembly.LoadFrom(file)
                         .GetTypes()
                         .Where(t => (typeof(IModule).IsAssignableFrom(t) || typeof(IServiceModule).IsAssignableFrom(t)) && !t.IsAbstract && t.IsClass);

                foreach (var type in moduleTypes)
                {
                    modules.Add(new ModuleDescription(type));
                }
            }
            return modules;
        }
    }
}
