using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.WPF.ViewService;

namespace xFrame.WPF.Modularity;

public sealed class ModuleInitializer : DefaultModuleInitializer
{
    private readonly IViewRegistrationService _viewRegistration;

    public ModuleInitializer(ITypeService typeService, IViewRegistrationService viewRegistration) : base(typeService)
    {
        _viewRegistration = viewRegistration;
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
            uiModule.RegisterViews(_viewRegistration);
        }
        module.Initialize(TypeService);
        moduleInfo.State = ModuleState.Initialized;
    }
}
