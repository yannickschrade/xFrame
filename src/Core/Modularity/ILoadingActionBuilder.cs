using System;

namespace xFrame.Core.Modularity
{
    public interface ILoadingActionBuilder<TMoulde>
        where TMoulde : IModule
    {
        ILoadingActionBuilder<TMoulde> AddExecute(Action<TMoulde> action);

    }

}
