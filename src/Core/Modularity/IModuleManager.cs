namespace xFrame.Core.Modularity;

public interface IModuleManager
{
    IEnumerable<IModuleInfo> LoadedModules { get; }
}