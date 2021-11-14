using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.WPF.ViewProvider;

namespace xFrame.WPF.Modularity;

internal sealed class ModuleInitializer : DefaultModuleInitializer
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
        if(module is IUiModule uiModule)
        {
            uiModule.RegisterViews(TypeService.Resolve<IViewRegistrationService>());
        }
        module.Initialize(TypeService);
        moduleInfo.State = ModuleState.Initialized;
    }
}
