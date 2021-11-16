using xFrame.Core.MVVM;

namespace xFrame.Core.ViewService
{
    public interface IViewContainer
    {
        object TargetView { get; }

        void Add(IViewFor view, IViewAdapter viewAdapter);

        void Remove(IViewFor view, IViewAdapter viewAdapter);

        bool IsLoaded { get; }

        void Initialize();
    }
}