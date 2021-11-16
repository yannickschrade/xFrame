using System.Windows;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewService
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
    }
}
