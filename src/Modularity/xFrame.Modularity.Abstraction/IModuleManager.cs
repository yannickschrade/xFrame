using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xFrame.Modularity.Abstraction
{
    public interface IModuleManager
    {
        event EventHandler<IModule> ModuleInitialisationStarted;
        event EventHandler<IModule> ModuleInitialized;

        List<IModule> RegisteredModules { get; }

        void InitializeModules();
        
    }
}