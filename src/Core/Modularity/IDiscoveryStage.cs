using System.Reflection;

namespace xFrame.Core.Modularity;

public interface IDiscoveryStage
{
    IDiscoveryStage AddModule(Type module);
    IDiscoveryStage AddModule<T>();
    IDiscoveryStage AddModulesFromAssembly(Assembly assembly);
    IDiscoveryStage AddModulesFromFolder(string path);
    IDiscoveryStage RemoveModule(string moduleName);
    IInitialisationStage UseModuleInitializer(IModuleInitializer moduleInitializer);
    ISortingStage SortModulesBy(Func<IEnumerable<IModuleInfo>, IEnumerable<IModuleInfo>> sortFunction);
    IModuleManager InitializeAll();
}
