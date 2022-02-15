using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    internal class LoadingPhaseBuilder<TModule> : ILoadingPhaseBuilder<TModule>
        where TModule : IModule
    {
        public LoadingPhase<TModule> LoadingPhase { get; }
        public ILoadingPhaseBuilder<TModule> AddLoadingAction(Action<ILoadingActionBuilder<TModule>> builder)
        {
            throw new NotImplementedException();
        }

        public ILoadingPhaseBuilder<TModule> AddLoadingAction(ILoadingAction<TModule> loadingAction)
        {
            LoadingPhase.AddAction(loadingAction);
            return this;
        }

        public ILoadingPhaseBuilder<TModule> Name(string name)
        {
            LoadingPhase.Name = name;
            return this;
        }
    }
}
