using System;

namespace xFrame.Core.Modularity
{
    public interface ILoadingPhaseBuilder<TModule>
      where TModule : IModule
    {
        ILoadingPhaseBuilder<TModule> AddLoadingAction(Action<TModule> action);
        ILoadingPhaseBuilder<TModule> AddLoadingAction(ILoadingAction<TModule> loadingAction);
        string Name { get; set; }
    }

}
