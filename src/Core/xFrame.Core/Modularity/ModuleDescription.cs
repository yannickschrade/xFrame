using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    public class ModuleDescription
    {
        public string Name { get; }
        public string Path { get; }
        public Type ImplementationType { get; }
        public ModuleType ModuleType { get; }

        public ModuleDescription(Type implementationType)
        {
            if (!typeof(IModule).IsAssignableFrom(implementationType) && !typeof(IServiceModule).IsAssignableFrom(implementationType))
                throw new ArgumentException($"Type must implement {typeof(IModule)} or {typeof(IServiceModule)}");

            Name = implementationType.Name;
            Path = implementationType.Assembly.Location;
            ImplementationType = implementationType;
            ModuleType = typeof(IModule).IsAssignableFrom(implementationType) ? ModuleType.FunctionalModule : ModuleType.ServiceModule;
        }

    }
}
