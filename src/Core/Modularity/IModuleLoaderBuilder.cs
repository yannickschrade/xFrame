using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace xFrame.Core.Modularity
{
    public interface IModuleLoaderBuilder<TModule>
        where TModule : IModule
    {
        string Name { get; set; }
        Func<Type, TModule> ModuleFactory { get; set; }

        IModuleLoaderBuilder<TModule> AddPhase(ILoadingPhase<TModule> phase);
        IModuleLoaderBuilder<TModule> AddPhase(object key,Action<ILoadingPhaseBuilder<TModule>> action);
    }

}
