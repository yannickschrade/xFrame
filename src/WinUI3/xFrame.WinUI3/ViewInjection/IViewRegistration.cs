using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xFrame.Core.MVVM;

namespace xFrame.WinUI3.ViewInjection
{
    public interface IViewRegistration
    {
        void Register(IViewFor view);
        void Register(Type viewType, Type viewModelType);
        void Register(Type viewType);
        void Register<TView, TViewModel>()
            where TView : FrameworkElement
            where TViewModel : ViewModelBase;

        void RegisterWindow<TWindow, TViewModel>()
            where TWindow : Window
            where TViewModel : ViewModelBase;
    }
}
