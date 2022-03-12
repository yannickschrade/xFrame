using System;

namespace xFrame.Modularity.Abstraction
{
    public interface IModuleLoaderCollection
    {
        IModuleLoader GetLoaderFor(IModule module);
        IModuleLoader GetLoaderFor(Type moduleType);
        void Add(IModuleLoader loader);
        bool TryAdd(Type type, IModuleLoader moduleLoader);
    }
}