using xFrame.Core.Modularity;
using xFrame.WPF.ViewService;

namespace xFrame.WPF.Modularity;

public interface IUiModule : IModule
{
    void RegisterViews(IViewRegistrationService registrationService);
}
