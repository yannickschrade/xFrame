using System;

namespace xFrame.Core.Modularity
{
    public class LoadingStep
    {
        

        public Type ModuleType { get; }
        public Action<object> Action { get; }
        public LoadingType LoadingType { get; }

        public LoadingStep(Type moduleType, Action<object> action, LoadingType loadingType)
        {
            ModuleType = moduleType;
            Action = action;
            LoadingType = loadingType;
        }
    }

    public class LoadingStep<T> : LoadingStep
        where T : IModule
    {
        public LoadingStep(Action<T> action, LoadingType loadingType)
            :base(typeof(T), p => action.Invoke((T)p), loadingType)
        {
        }
    }
}