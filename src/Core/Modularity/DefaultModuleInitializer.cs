using xFrame.Core.IoC;

namespace xFrame.Core.Modularity;

internal class DefaultModuleInitializer : IModuleInitializer
{

    private readonly ITypeService _typeService;

    public DefaultModuleInitializer(ITypeService typeService)
    {
        ArgumentNullException.ThrowIfNull(typeService, nameof(typeService));
        _typeService = typeService;
    }

    public bool CanInitializeModule(IModuleInfo moduleInfo)
    {
        return true;
    }

    public void InitializeModule(IModuleInfo moduleInfo)
    {
        if (moduleInfo is null || moduleInfo.State == ModuleState.Initialized)
        {
            return;
        }
        moduleInfo.State = ModuleState.Loading;
        var module = CreateModule(moduleInfo);
        moduleInfo.State = ModuleState.RegisteringTypes;
        moduleInfo.State = ModuleState.Initializing;
        module.RegisterServices(_typeService);
        module.Initialize(_typeService);
        moduleInfo.State = ModuleState.Initialized;
    }

    private IModule CreateModule(IModuleInfo moduleInfo)
    {
        return (IModule)_typeService.Resolve(moduleInfo.Type);
    }
}
