namespace xFrame.WPF.ViewProvider;

public interface IViewRegistrationService
{
    IViewRegistrationService RegisterViewWithViewModel(Type viewType, Type viewModelType);

    IViewRegistrationService RegisterView(Type viewType);

    IViewRegistrationService RegisterWindow(Type Window);

    IViewRegistrationService RegisterWindowWithViewModel(Type windowType, Type viewModelType);
}
