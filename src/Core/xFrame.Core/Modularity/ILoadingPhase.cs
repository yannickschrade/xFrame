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
        ILoadingPhase<TModule> AddLoadingAction(ILoadingAction<TModule> action);
        ILoadingActionBuilder<TModule> AddLoadingAction(Action<TModule> action);
    }

}
