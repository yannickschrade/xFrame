using System.Linq;
using System.Reflection;
using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.Core.MVVM;
using xFrame.Core.ViewService;
using xFrame.WPF.ViewInjection;
using xFrame.WPF.ViewService;

namespace xFrame.WPF.Modularity
{
    public sealed class ModuleInitializer : DefaultModuleInitializer
    {

        public ModuleInitializer(ITypeService typeService) : base(typeService)
        {
        }

        public override void InitializeModule(IModuleInfo moduleInfo)
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
            if (module is IUiModule uiModule)
            {
                var moduleAssembly = moduleInfo.ModuleAssembly;
                RegisterViews(moduleAssembly);
                uiModule.SetupUI(TypeService.Resolve<IViewManager>(), TypeService.Resolve<IViewAdapterCollection>());
            }
            module.OnInitialized(TypeService);
            moduleInfo.State = ModuleState.Initialized;
        }

        private void RegisterViews(Assembly moduleAssembly)
        {
            var viewTypes = moduleAssembly.GetTypes().Where(t => typeof(IViewFor).IsAssignableFrom(t));
            var viewRegistrationService = TypeService.Resolve<IViewRegistrationService>();
            foreach (var viewType in viewTypes)
            {
                viewRegistrationService.RegisterView(viewType);
            }
        }
    }
}