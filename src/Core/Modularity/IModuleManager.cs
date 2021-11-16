using xFrame.Core.IoC;

namespace xFrame.Core.Modularity;

public interface IModuleManager
{
    IEnumerable<IModuleInfo> LoadedModules { get; }
    void Run();
}