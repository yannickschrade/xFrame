using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    public class LoadingPhase<TModule> : ILoadingPhase<TModule>
        where TModule : IModule
    {
        private List<ILoadingAction<TModule>> _loadingActions = new List<ILoadingAction<TModule>>();

        public object Key { get; }
        public IEnumerable<ILoadingAction<TModule>> LoadingActions => _loadingActions;

        public string Name { get; set; }

        public LoadingPhase(object key)
        {
            Key = key;
            Name = ToString();
        }

        public ILoadingPhase<TModule> AddLoadingAction(ILoadingAction<TModule> action)
        {
            _loadingActions.Add(action);
            return this;
        }

        public void Run(TModule module)
        {
            foreach (var action in _loadingActions)
            {
                action.Execute(module);
            }
        }

        public ILoadingActionBuilder<TModule> AddLoadingAction(Action<TModule> action)
        {
            var builder = new LoadingActionBuilder<TModule>(action);
            _loadingActions.Add(builder.LoadingAction);
            return builder;

        }
    }
}
