using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using xFrame.Modularity.Abstraction;

namespace xFrame.WPF.Modularity
{
    public static class ModuleCollectionExtensions
    {

        public static IModuleCollection AddMoulde(this IModuleCollection modules, Type moduleType)
        {
            if (!typeof(IModule).IsAssignableFrom(moduleType) || moduleType.IsAbstract || !moduleType.IsClass)
                throw new ArgumentException(nameof(moduleType) + " must be an non abstract class which implements the " + typeof(IModule) + " interface");

            if (moduleType.GetConstructor(Type.EmptyTypes) == null)
                throw new ArgumentException(nameof(moduleType) + " must have an parameterless constructor");

            modules.Add(new ModuleDescription(moduleType));
            return modules;
        }
        

        public static IModuleCollection AddModule<T>(this IModuleCollection modules)
            where T : IModule, new ()
        {
            return modules.AddMoulde(typeof(T));
        }

        public static IModuleCollection AddFromJson(this IModuleCollection modules, string jsonPath)
        {
            var json = File.ReadAllText(jsonPath, Encoding.UTF8);
            var jsonModules = JsonSerializer.Deserialize<ModuleCollection>(json);

            modules.AddFromCollection(jsonModules);
            return modules;
        }

        public static IModuleCollection AddFromFolder(this IModuleCollection modules, string path)
        {
            var directories = Directory.GetDirectories(path);
            if (!directories.Any())
                throw new InvalidOperationException("Modules must be in a sub folder per module");
            foreach (var directory in directories)
            {
                var dirname = Path.GetDirectoryName(directory);
                foreach (var file in Directory.GetFiles(directory, "*" + dirname + "*.dll"))
                {
                    var name = Path.GetFileName(file);
                    modules.Add(new ModuleDescription(name, file));
                }
            }

            return modules;
        }
    }
}
