using System;
using System.Windows;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewInjection
{
    public interface IViewProvider
    {
        FrameworkElement GetViewForViewModel(ViewModelBase vm);
        FrameworkElement GetViewForViewModel(Type viewModelType);
        FrameworkElement GetViewForViewModel<T>()
            where T : ViewModelBase;

        void Register(IViewFor view);
        void Register(Type viewType, Type viewModelType);
        void Register(Type viewType);
        void Register<TView, TViewModel>()
            where TView : FrameworkElement
            where TViewModel : ViewModelBase;

    }
}
