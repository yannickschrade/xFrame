using System.Collections.Generic;

namespace xFrame.Core.Modularity
{
    public interface IModuleManager
    {
        IEnumerable<IModuleInfo> LoadedModules { get; }
        void Run();
    }
}