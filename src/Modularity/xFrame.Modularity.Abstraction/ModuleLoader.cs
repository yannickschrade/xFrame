using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace xFrame.Modularity.Abstraction
{
    public abstract class ModuleLoader<T> : IModuleLoader
        where T : IModule
    {
        public Type ForModuleType => typeof(T);
        public abstract IModule CreateModule(HostBuilderContext context, IServiceCollection services, Type moduleType);

        protected abstract void InitializeModule(IServiceProvider services, T module);

        public void InitializeModule(IServiceProvider services, object module)
        {
            InitializeModule(services, (T) module);
        }
    }
}