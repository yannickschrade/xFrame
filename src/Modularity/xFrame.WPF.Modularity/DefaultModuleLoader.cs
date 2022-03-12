using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xFrame.Modularity.Abstraction;

namespace xFrame.WPF.Modularity
{
    public class DefaultModuleLoader : ModuleLoader<IModule>
    {
    

        public override IModule CreateModule(HostBuilderContext context, IServiceCollection services, Type moduleType)
        {
            var module = (IModule)Activator.CreateInstance(moduleType);
        
            if (module == null) return null;
        
            module.RegisterServices(services);
            return module;
        }

        protected override void InitializeModule(IServiceProvider services, IModule module)
        {
            module.OnLoaded(services);
        }
    }    
}

