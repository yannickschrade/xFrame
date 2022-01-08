using System;
using System.Collections.Generic;
using System.Threading;
using xFrame.Core.IoC;

namespace xFrame.Core.Modularity
{
    public class DefaultModuleInitializer : IModuleInitializer
    {

        protected readonly ITypeService TypeService;

        protected List<Action<IModuleInfo>> Steps;

        public IEnumerable<Action<IModuleInfo>> InitializationSteps => Steps;

        public DefaultModuleInitializer(ITypeService typeService)
        {
            Steps = new List<Action<IModuleInfo>>
            {
                RegisterTypes,
                InitializeModule,
            };

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


        protected void InitializeModule(IModuleInfo moduleInfo)
        {
            if (moduleInfo is null || moduleInfo.State == ModuleState.Initialized)
            {
                return;
            }
            
            moduleInfo.State = ModuleState.Initializing;
            moduleInfo.Instance.OnInitialized(TypeService);
            moduleInfo.State = ModuleState.Initialized;
        }

        protected void RegisterTypes(IModuleInfo moduleInfo)
        {
            if (moduleInfo is null || moduleInfo.State == ModuleState.Initialized)
            {
                return;
            }
            moduleInfo.State = ModuleState.RegisteringTypes;
            var module = CreateModule(moduleInfo);
            module.RegisterServices(TypeService);
            moduleInfo.Instance = module;
        }

        protected virtual IModule CreateModule(IModuleInfo moduleInfo)
        {
            moduleInfo.State = ModuleState.Loading;
            return (IModule)TypeService.Resolve(moduleInfo.Type);
        }
    }
}