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

       

    }
}
