using System;
using System.Collections;
using System.Collections.Generic;

namespace xFrame.Core.Modularity
{
    public interface IModuleInitializer
    {
        IEnumerable<Action<IModuleInfo>> InitializationSteps { get; }
        bool CanInitializeModule(IModuleInfo moduleInfo);
    }
}