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

        IModule CreateModule(Type moduleType);

        void LoadModule(IModule module);
    }

    public interface IModuleLoader<TModule> : IModuleLoader
        where TModule : IModule
    {
        new IEnumerable<ILoadingPhase<TModule>> LoadingPhases { get; }

        IModuleLoader<TModule> AddPhase(Action<ILoadingPhaseBuilder<TModule>> builder);
        IModuleLoader<TModule> AddPhase(ILoadingPhase<TModule> loadingPhase);

        new TModule CreateModule(Type moduleType);

        void LoadModule(TModule module);
    }

}
