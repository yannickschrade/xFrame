using xFrame.Core.Modularity;
using xFrame.Core.ViewInjection;
using xFrame.WPF.ViewInjection;

namespace xFrame.WPF.Modularity
{
    public interface IUiModule : IModule
    {
        void SetupViews(IViewInjectionService viewInjectionService);
    }
}