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
        IModuleLoaderBuilder<TModule> Name(string name);
        IModuleLoaderBuilder<TModule> Factory(Func<Type, TModule> factory);
        IModuleLoaderBuilder<TModule> AddPhase(ILoadingPhase<TModule> phase);
        IModuleLoaderBuilder<TModule> AddPhase(Action<ILoadingPhaseBuilder<TModule>> action);
    }

}
