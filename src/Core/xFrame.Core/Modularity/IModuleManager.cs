using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xFrame.Core.Modularity
{
    public interface IModuleManager
    {
        event EventHandler<IServiceModule> ModuleInitialisationStarted;
        event EventHandler<IServiceModule> ModuleInitialized;

        List<IServiceModule> RegisteredModules { get; }

        void InitializeModules();

    }
}