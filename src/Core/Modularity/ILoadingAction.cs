using System;

namespace xFrame.Core.Modularity
{
    public interface ILoadingAction<TModule>
        where TModule : IModule
    {
        void AddAction(Action<TModule> action);
        void Execute(TModule module);
    }

}
