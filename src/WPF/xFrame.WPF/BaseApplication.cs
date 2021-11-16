using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.WPF.ViewAdapters;
using xFrame.WPF.ViewInjection;
using xFrame.WPF.ViewService;

namespace xFrame.WPF;

public abstract class BaseApplication : Application
{
    protected ITypeService TypeService;

    protected abstract ITypeService CreateTypeService();
    protected abstract Window CreateShell(IViewProviderService viewProvider);
    protected abstract void RegisterTypes(ITypeRegistrationService typeRegistration);

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        SetupApp();
        InitializeShell(CreateShell(TypeService.Resolve<IViewProviderService>()));
        OnInitialized();
        ShowShell();
    }

    protected virtual void  ShowShell()
    {
        MainWindow.Loaded += (s, e) => OnShellLoaded();
        MainWindow.Show();
    }

    protected virtual void SetupApp()
    {
        TypeService = CreateTypeService();
        RegisterDefaultTypes(TypeService);
        RegisterTypes(TypeService);
        RegisterDefaultViewAdapters(TypeService.Resolve<IViewInjectionService>());
        RegisterViewAdapters(TypeService.Resolve<IViewInjectionService>());
        RegisterViews(TypeService.Resolve<IViewRegistrationService>());
    }

    private void RegisterDefaultViewAdapters(IViewInjectionService viewInjectionService)
    {
        viewInjectionService.RegisterAdapterIfMissing<ContentControlAdapter>();
        viewInjectionService.RegisterAdapterIfMissing<ItemsControlAdapter>();
        viewInjectionService.RegisterAdapterIfMissing<PanelAdapter>();
    }

    private void RegisterDefaultTypes(ITypeRegistrationService typeService)
    {
        typeService.RegisterSingeltonMany<ViewService.ViewService>(typeof(IViewRegistrationService), typeof(IViewProviderService))
            .RegisterSingelton<ViewInjectionService, IViewInjectionService>();
    }

    protected virtual void RegisterViewAdapters(IViewInjectionService viewInjection)
    {
        var adpaters = Assembly.GetEntryAssembly()
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IViewAdapter)))
            .Where( t=> t.IsClass)
            .Where(t => !t.IsAbstract);

        viewInjection.RegisterAdaptersIfMissing(adpaters);
    }

    protected virtual void InitializeShell(Window shell)
    {
        MainWindow = shell;
    }

    protected virtual void OnShellLoaded()
    {
        TypeService.Resolve<IViewInjectionService>().Initialize();
    }

    protected virtual void RegisterViews(IViewRegistrationService viewRegistrationService)
    {
        var views = Assembly.GetEntryAssembly()
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(Window)) || t.IsAssignableTo(typeof(UserControl)));

        foreach (var view in views)
        {
            viewRegistrationService.RegisterView(view);
        }
    }

    protected virtual void OnInitialized() { }

}
