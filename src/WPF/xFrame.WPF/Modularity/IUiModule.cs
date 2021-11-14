using xFrame.Core.Modularity;
using xFrame.WPF.ViewProvider;

namespace xFrame.WPF.Modularity;

public interface IUiModule : IModule
{
    void RegisterViews(IViewRegistrationService registrationService);
}
