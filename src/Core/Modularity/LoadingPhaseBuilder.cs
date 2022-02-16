using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    internal class LoadingPhaseBuilder<TModule> : ILoadingPhaseBuilder<TModule>
        where TModule : IModule
    {
        public LoadingPhase<TModule> LoadingPhase { get; }
        public string Name
        {
            get => LoadingPhase.Name;
            set => LoadingPhase.Name = value;
        }

        public LoadingPhaseBuilder(LoadingPhase<TModule> loadingPhase)
        {
            LoadingPhase = loadingPhase;
        }

        public ILoadingPhaseBuilder<TModule> AddLoadingAction(Action<TModule> action)
        {
            LoadingPhase.AddLoadingAction(action);
            return this;
        }

        public ILoadingPhaseBuilder<TModule> AddLoadingAction(ILoadingAction<TModule> loadingAction)
        {
            LoadingPhase.AddLoadingAction(loadingAction);
            return this;
        }
    }
}
