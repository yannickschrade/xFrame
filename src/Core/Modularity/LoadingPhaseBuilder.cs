using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    internal class LoadingPhaseBuilder<TModule> : ILoadingPhaseBuilder<TModule>
        where TModule : IModule
    {
        public LoadingPhase<TModule> LoadingPhase { get; private set; }
        public ILoadingPhaseBuilder<TModule> AddLoadingAction(object key, Action<ILoadingActionBuilder<TModule>> action)
        {
            var builder = new LoadingActionBuilder<TModule>();
            action(builder);
            LoadingPhase = new LoadingPhase<TModule>(key);
            LoadingPhase.AddAction(builder.LoadingAction);
            return this;
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
