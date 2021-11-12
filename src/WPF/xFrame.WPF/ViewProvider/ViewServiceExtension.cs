using System.Windows;
using xFrame.Core.MVVM;
using xFrame.WPF.ViewProvider;

namespace xFrame.WPF.ViewInjection
{
    public static class ViewServiceExtension
    {
        public static IViewRegistrationService RegisterView<TView>(this IViewRegistrationService registrationService)
            where TView : UIElement
        {
            registrationService.RegisterView(typeof(TView));
            return registrationService;
        }

        public static IViewRegistrationService RegisterViewWithViewModel<TView, TViewModel>(this IViewRegistrationService registrationService)
            where TView : UIElement
            where TViewModel : ViewModelBase
        {
            registrationService.RegisterViewWithViewModel(typeof(TView), typeof(TViewModel));
            return registrationService;
        }

        public static IViewRegistrationService RegisterWindow<TWindow>(this IViewRegistrationService registrationService)
            where TWindow : Window
        {
            registrationService.RegisterWindow(typeof(TWindow));
            return registrationService;
        }

        public static IViewRegistrationService RegisterWindowWithViewModel<TWindow, TViewModel>(this IViewRegistrationService registrationService)
            where TWindow : Window
            where TViewModel : ViewModelBase
        {
            registrationService.RegisterWindowWithViewModel(typeof(TWindow), typeof(TViewModel));
            return registrationService;
        }


    }
}
