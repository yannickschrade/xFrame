using System;
using System.Collections.Generic;
using xFrame.Modularity.Abstraction;

namespace xFrame.WPF.Modularity.@internal
{
    public class ModuleLoaderCollection : IModuleLoaderCollection
    {
        private Dictionary<Type, IModuleLoader> _loaders = new Dictionary<Type, IModuleLoader>();
        public IModuleLoader GetLoaderFor(IModule module)
        {
            var moduleType = module.GetType();
            var baseType = moduleType;
            while (baseType != null)
            {
                if (_loaders.TryGetValue(baseType, out var moduleLoader))
                {
                    return moduleLoader;
                }

                baseType = baseType.BaseType;
            }

            var interfaces = moduleType.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (_loaders.TryGetValue(@interface, out var moduleLoader))
                {
                    return moduleLoader;
                }
            }
            
            return null;
        }

        public IModuleLoader GetLoaderFor(Type moduleType)
        {
            var baseType = moduleType;
            while (baseType != null)
            {
                if (_loaders.TryGetValue(baseType, out var moduleLoader))
                {
                    return moduleLoader;
                }

                baseType = baseType.BaseType;
            }

            var interfaces = moduleType.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (_loaders.TryGetValue(@interface, out var moduleLoader))
                {
                    return moduleLoader;
                }
            }

            return null;
        }

        public void Add(IModuleLoader loader)
        {
            _loaders.Add(loader.ForModuleType, loader);
        }

        public bool TryAdd(Type type, IModuleLoader moduleLoader)
        {
            return _loaders.TryAdd(type, moduleLoader);
        }
    }
}
