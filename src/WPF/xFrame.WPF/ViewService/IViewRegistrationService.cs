namespace xFrame.WPF.ViewService;

public interface IViewRegistrationService
{
    IViewRegistrationService RegisterViewWithViewModel(Type viewType, Type viewModelType);

    IViewRegistrationService RegisterView(Type viewType);
}
