using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    internal class LoadingActionBuilder<TModule> : ILoadingActionBuilder<TModule>
        where TModule : IModule
    {
        public LoadingAction<TModule> LoadingAction { get; }

        public LoadingActionBuilder(Action<TModule> action)
        {
            LoadingAction = new LoadingAction<TModule>(action);
        }
    }
}
