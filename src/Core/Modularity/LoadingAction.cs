using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    public class LoadingAction<TModule> : ILoadingAction<TModule>
        where TModule : IModule    
    {
        private Action<TModule> _action;
        public void AddAction(Action<TModule> action)
        {
            _action = action;
        }

        public void Execute(TModule module)
        {
            _action(module);
        }
    }
}
