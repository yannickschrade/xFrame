using Microsoft.UI.Xaml;
using System;
using xFrame.Core.MVVM;

namespace xFrame.WinUI3.ViewInjection
{
    public interface IViewProvider
    {
        FrameworkElement GetViewWithViewModel(ViewModelBase vm);
        FrameworkElement GetViewWithViewModel(Type viewModelType);
        FrameworkElement GetViewWithViewModel<T>()
            where T : ViewModelBase;

        Window GetWindowWithViewModel(ViewModelBase vm);
        Window GetWindowWithViewModel(Type viewModelType);
        Window GetWindowWithViewModel<T>();
    }
}
