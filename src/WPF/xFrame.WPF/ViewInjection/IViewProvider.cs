using System;
using System.Windows;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewInjection
{
    public interface IViewProvider
    {
        FrameworkElement GetViewForViewModel(IViewModel vm);
        FrameworkElement GetViewForViewModel(Type viewModelType);
        FrameworkElement GetView(Type viewType);
        T GetView<T>()
            where T : FrameworkElement;
        FrameworkElement GetViewForViewModel<T>()
            where T : IViewModel;

       

    }
}
