using System.Windows;
using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.WPF.Controls;
using xFrame.WPF.Modularity;
using xFrame.WPF.ViewService;

namespace xFrame.WPF;

public abstract class XFrameModularApp : BaseApplication
{
    protected abstract override Window CreateShell(IViewProviderService viewProvider);
    protected abstract override void RegisterTypes(ITypeRegistrationService typeRegistration);
    protected abstract IDiscoveryStage AddModules(IDiscoveryStage moduleManager);
    protected virtual SplashWindow? CreateSplashWindow(IViewProviderService viewProvider) { return null; }

    protected override ITypeService CreateTypeService()
    {
        return new DryIocContainerWrapper();
    }

    protected override void SetupApp()
    {
        base.SetupApp();
        var moduleManager = SetupModuleManager(AddModules(CreateModuleManager()));
        var splashWindow = CreateSplashWindow(TypeService.Resolve<IViewProviderService>());
        var vm = splashWindow?.DataContext;
        splashWindow?.Show();
        moduleManager.Run();
    }

    protected virtual IDiscoveryStage CreateModuleManager()
    {
        return ModuleManager.Create(TypeService);
    }

    protected virtual IModuleManager SetupModuleManager(IDiscoveryStage moduleManager)
    {
        return moduleManager.UseModuleInitializer<ModuleInitializer>();
    }

}
