using System;
using xFrame.Core.MVVM;

namespace xFrame.WinUI3.ViewInjection
{
    public interface IViewFor
    {
        Type ViewModelType { get; }
        ViewModelBase ViewModel { get; set; }
    }

    public interface IViewFor<T> : IViewFor
        where T : ViewModelBase
    {
        Type IViewFor.ViewModelType => typeof(T);
        new T ViewModel { get; set; }
    }
}
