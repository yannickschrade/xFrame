using System.Windows;
using xFrame.Core.IoC;
using xFrame.WPF.ViewService;

namespace xFrame.WPF;

public abstract class XFrameApp : BaseApplication
{
    protected abstract override Window CreateShell(IViewProviderService viewProvider);

    protected override ITypeService CreateTypeService()
    {
        return new DryIocContainerWrapper();
    }

    protected abstract override void RegisterTypes(ITypeRegistrationService typeRegistration);
}
