using System;
using System.Collections.Generic;
using System.Reflection;

namespace xFrame.Core.Modularity
{
    public interface IModuleLoader
    {
        string Name { get; }
        Type ForType { get; }
        Func<Type, object> ModuleFactory { get; }
        IEnumerable<ILoadingPhase<IModule>> LoadingPhases { get; }
        IModule CreateModule(Type moduleType);
        void LoadModule(IModule module);
    }

    public interface IModuleLoader<TModule> : IModuleLoader 
        where TModule : IModule
    {
        new Func<Type, TModule> ModuleFactory { get; }
        new IEnumerable<ILoadingPhase<TModule>> LoadingPhases { get; }
        new TModule CreateModule(Type moduleType);
        void LoadModule(TModule module);
        IModuleLoader<TModule> AddPhase(ILoadingPhase<TModule> phase);
        IModuleLoader<TModule> AddPhase(object key, Action<ILoadingPhaseBuilder<TModule>> phase);
        IModuleLoader<TModule> EditPhase(object key, Action<ILoadingPhase<TModule>> phase);
    }

}
