using System;

namespace xFrame.Core.Modularity
{
    public interface ILoadingAction<TModule>
        where TModule : IModule
    {
        void Execute(TModule module);
    }

}
