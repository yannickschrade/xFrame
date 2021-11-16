using System;
using System.Windows;
using xFrame.Core.ViewService;

namespace xFrame.WPF.ViewService
{
    public interface IViewRegistrationService
    {
        void RegisterView(Type viewType);
        void RegisterView<T>()
           where T : UIElement, IViewFor, new();
    }
}