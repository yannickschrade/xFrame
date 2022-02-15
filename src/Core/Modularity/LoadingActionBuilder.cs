using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    internal class LoadingActionBuilder<TModule> : ILoadingActionBuilder<TModule>
        where TModule : IModule
    {
        public LoadingAction<TModule> LoadingAction { get; }
        public ILoadingActionBuilder<TModule> AddExecute(Action<TModule> action)
        {
            LoadingAction.AddAction(action);
            return this;
        }
    }
}
