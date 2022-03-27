using System;
using Microsoft.Extensions.DependencyInjection;

namespace xFrame.Core.Modularity
{
    public abstract class ModuleLoader<T> : IModuleLoader
        where T : IModule
    {
        public Type ForModuleType => typeof(T);
        public abstract IModule CreateModule(IServiceProvider services, Type moduleType);

        protected abstract void InitializeModule(IServiceProvider services, T module);

        public void InitializeModule(IServiceProvider services, object module)
        {
            InitializeModule(services, (T)module);
        }
    }
}