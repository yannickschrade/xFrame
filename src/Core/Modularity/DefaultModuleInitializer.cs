using System;
using System.Threading;
using xFrame.Core.IoC;

namespace xFrame.Core.Modularity
{
    public class DefaultModuleInitializer : IModuleInitializer
    {

        protected readonly ITypeService TypeService;

        public DefaultModuleInitializer(ITypeService typeService)
        {
            if(typeService == null)
            {
                throw new ArgumentNullException(nameof(typeService));
            }
            TypeService = typeService;
        }

        public virtual bool CanInitializeModule(IModuleInfo moduleInfo)
        {
            return true;
        }

        public virtual void InitializeModule(IModuleInfo moduleInfo)
        {
            if (moduleInfo is null || moduleInfo.State == ModuleState.Initialized)
            {
                return;
            }
            moduleInfo.State = ModuleState.Loading;
            var module = CreateModule(moduleInfo);
            moduleInfo.State = ModuleState.RegisteringTypes;
            moduleInfo.State = ModuleState.Initializing;
            module.RegisterServices(TypeService);
            module.Initialize(TypeService);
            moduleInfo.State = ModuleState.Initialized;
        }

        protected virtual IModule CreateModule(IModuleInfo moduleInfo)
        {
            return (IModule)TypeService.Resolve(moduleInfo.Type);
        }
    }
}