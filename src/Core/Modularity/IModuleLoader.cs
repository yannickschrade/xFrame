using System;
using System.Collections.Generic;
using System.Reflection;

namespace xFrame.Core.Modularity
{
    public interface IModuleLoader
    {
        IEnumerable<ILoadingPhase<IModule>> LoadingPhases { get; }
        Type ForType { get; }
        string Name { get; }
        Func<Type, object> ModuleFactory { get; }
    }

    public interface IModuleLoader<TModule> : IModuleLoader
        where TModule : IModule
    {
        new Func<Type, TModule> ModuleFactory { get; }
        new IEnumerable<ILoadingPhase<TModule>> LoadingPhases { get; }

        IModuleLoader<TModule> AddPhase(Action<ILoadingPhaseBuilder<TModule>> builder);
        IModuleLoader<TModule> AddPhase(ILoadingPhase<TModule> loadingPhase);
    }

}
