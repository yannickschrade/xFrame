using Microsoft.UI.Xaml;
using System;
using xFrame.Core.MVVM;

namespace xFrame.WinUI3.ViewInjection
{
    public interface IViewProvider
    {
        FrameworkElement GetViewWithViewModel(IViewModel vm);
        FrameworkElement GetViewWithViewModel(Type viewModelType);
        FrameworkElement GetViewWithViewModel<T>()
            where T : IViewModel;

        Window GetWindowWithViewModel(IViewModel vm);
        Window GetWindowWithViewModel(Type viewModelType);
        Window GetWindowWithViewModel<T>();
    }
}
