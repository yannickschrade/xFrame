using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewInjection
{
    public interface IViewRegistration
    {
        void Register(IViewFor view);
        void Register(Type viewType, Type viewModelType);
        void Register(Type viewType);
        void Register<TView, TViewModel>()
            where TView : FrameworkElement
            where TViewModel : ViewModelBase;
    }
}
