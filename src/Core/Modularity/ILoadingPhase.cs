using System;
using System.Collections.Generic;

namespace xFrame.Core.Modularity
{
    public interface ILoadingPhase<TModule>
        where TModule : IModule
    {
        string Name { get; }
        object Key { get; }
        IEnumerable<ILoadingAction<TModule>> LoadingActions { get;}
        
        void Run(TModule module);
        ILoadingPhase<TModule> AddAction(ILoadingAction<TModule> action);
        ILoadingPhase<TModule> AddAction(Action<ILoadingActionBuilder<TModule>> action);
    }

}
