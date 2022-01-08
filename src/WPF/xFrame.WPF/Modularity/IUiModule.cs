using xFrame.Core.Modularity;
using xFrame.Core.ViewService;

namespace xFrame.WPF.Modularity
{
    public interface IUiModule : IModule
    {
        void SetupUI(IViewManager viewManager, IViewAdapterCollection viewAdapterCollection);
    }
}