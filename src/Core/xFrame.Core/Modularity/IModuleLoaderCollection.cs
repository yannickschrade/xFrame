using System;

namespace xFrame.Core.Modularity
{
    public interface IModuleLoaderCollection
    {
        IModuleLoader GetLoaderFor(IModule module);
        IModuleLoader GetLoaderFor(Type moduleType);
        void Add(IModuleLoader loader);
        bool TryAdd(Type type, IModuleLoader moduleLoader);
    }
}