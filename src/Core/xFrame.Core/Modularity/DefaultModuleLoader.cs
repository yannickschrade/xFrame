using System;
using Microsoft.Extensions.DependencyInjection;
using xFrame.Core.ExtensionMethodes;

namespace xFrame.Core.Modularity
{
    public class DefaultModuleLoader : ModuleLoader<IModule>
    {
        public override IModule CreateModule(IServiceProvider services, Type moduleType)
        {
            return (IModule)services.GetUnregistredService(moduleType);
        }

        protected override void InitializeModule(IServiceProvider services, IModule module)
        {
            module.Initialize(services);
        }
    }
}

