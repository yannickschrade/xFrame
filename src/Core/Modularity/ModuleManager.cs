using System.Reflection;
using xFrame.Core.Attributes;
using xFrame.Core.IoC;

namespace xFrame.Core.Modularity;

public class ModuleManager :
    IDiscoveryStage,
    ISortingStage,
    IInitialisationStage,
    IModuleManager
{
    private readonly List<IModuleInfo> _modules = new();

    private IModuleInitializer? _moduleInitializer;
    private IEnumerable<IModuleInfo>? _sortedModules;

    private ModuleManager(ITypeService typeService)
    {
        _moduleInitializer = new DefaultModuleInitializer(typeService);
    }

    public IEnumerable<IModuleInfo> LoadedModules => _sortedModules ?? _modules;

    public static IDiscoveryStage Create(ITypeService typeService)
    {
        return new ModuleManager(typeService);
    }


    public IDiscoveryStage AddModule(Type module)
    {
        var info = CreateModuleInfo(module);
        if(_modules.Any(m => m.Name == info.Name))
        {
            return this;
        }

        _modules.Add(info);
        return this;
    }

    public IDiscoveryStage AddModule<T>()
    {
        return AddModule(typeof(T));
    }

    public IDiscoveryStage AddModulesFromAssembly(Assembly assembly)
    {
        var modules = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IModule)))
            .Where(t => t.IsClass);

        foreach (var module in modules)
        {
            if (_modules.Any(m => m.Name == module.Name))
            {
                continue;
            }
            _modules.Add(CreateModuleInfo(module));
        }

        return this;
    }

    public IDiscoveryStage AddModulesFromFolder(string path)
    {
        foreach (var dll in Directory.GetFiles(path, "*.dll"))
        {
            var assembly = Assembly.Load(dll);
            AddModulesFromAssembly(assembly);
        }

        return this;
    }

    public IModuleManager InitializeAll()
    {
        if(_sortedModules == null)
        {
            throw new InvalidOperationException("Modules not sorted");
        }

        if(_moduleInitializer is null)
        {
            throw new InvalidOperationException("moduleinitilizer is not setted");
        }


        foreach (var module in _sortedModules)
        {
            if (!_moduleInitializer.CanInitializeModule(module))
            {
                throw new InvalidOperationException("ModuleInitializer can't initialize module");
            }
            _moduleInitializer.InitializeModule(module);
        }

        return this;
    }

    IModuleManager IDiscoveryStage.InitializeAll()
    {
        _sortedModules = SortModules();
        InitializeAll();
        return this;
    }

    public IDiscoveryStage RemoveModule(string moduleName)
    {
        _modules.RemoveAll(m => m.Name == moduleName);
        return this;
    }
    public ISortingStage SortModulesBy(Func<IEnumerable<IModuleInfo>, IEnumerable<IModuleInfo>> sortFunction)
    {
        _sortedModules = sortFunction(_modules);
        return this;
    }

    public IInitialisationStage UseModuleInitializer(IModuleInitializer moduleInitializer)
    {
        _moduleInitializer = moduleInitializer;
        return this;
    }

    private ModuleInfo CreateModuleInfo(Type moduleType)
    {
        if (!moduleType.IsAssignableTo(typeof(IModule)))
        {
            throw new InvalidOperationException($"{moduleType.FullName} does not implement IModule");
        }

        ArgumentNullException.ThrowIfNull(moduleType, nameof(moduleType));
        var name = moduleType.Name;

        var moduleAttribute = moduleType.GetCustomAttribute<ModuleAttribute>();
        name = moduleAttribute?.ModuleName;
        var version = moduleAttribute?.Version ?? new Version();
        var priority = moduleAttribute?.Priority ?? 0;
        var type = moduleAttribute?.ModuleType ?? ModuleType.Undefined;

        var verAttr = moduleType.GetCustomAttribute<ModuleVersionAttribute>();
        var typeAttr = moduleType.GetCustomAttribute<ModuleTypeAttribute>();
        var prioAttr = moduleType.GetCustomAttribute<ModulePriorityAttribute>();

        if (verAttr is not null)
        {
            version = verAttr.ModuleVersion;
        }

        if (typeAttr is not null)
        {
            type = typeAttr.ModuleType;
        }

        if (prioAttr is not null)
        {
            priority = prioAttr.Priority;
        }

        return new ModuleInfo(moduleType, name!, version, priority, type);
    }

    IInitialisationStage IDiscoveryStage.UseModuleInitializer(IModuleInitializer moduleInitializer)
    {
        _sortedModules = SortModules();
        return UseModuleInitializer(moduleInitializer);
    }

    private IEnumerable<IModuleInfo> SortModules()
    {
        return _modules.OrderBy(m => m.ModuleType)
            .ThenBy(m => m.Priority)
            .ThenBy(m => m.Name);

    }
}
