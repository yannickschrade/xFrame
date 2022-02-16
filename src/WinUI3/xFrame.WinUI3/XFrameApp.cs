using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System.Linq;
using System.Reflection;
using xFrame.Core.IoC;
using xFrame.Core.ViewInjection;
using xFrame.WinUI3.ExtensionMethodes;
using xFrame.WinUI3.ViewAdapters;
using xFrame.WinUI3.ViewInjection;

namespace xFrame.WinUI3
{
    public abstract class XFrameApp : Application
    {

        protected Window Window;
        protected AppWindow AppWindow;

        protected ITypeService TypeService;
        

        protected abstract void RegisterTypes(ITypeRegistrationService typeRegistration);

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            SetupApp();
            InitializeShell(CreateShell(TypeService.Resolve<IViewProvider>()));
            InitializeApp();
            ShowShell();
        }

        protected virtual ITypeService CreateTypeService()
        {
            return new DryIocContainerWrapper();
        }

        protected virtual void ShowShell()
        {
            Window.Activate();
        }

        protected virtual void SetupApp()
        {
            TypeService = CreateTypeService();
            TypeProvider.Current = TypeService;
            RegisterDefaultTypes(TypeService);
            RegisterTypes(TypeService);
            RegisterDefaultViewAdapters(TypeService.Resolve<IViewAdapterCollection>());
            RegisterViewAdapters(TypeService.Resolve<IViewAdapterCollection>());
            RegisterViews(TypeService.Resolve<IViewRegistration>());
        }

        private void RegisterDefaultViewAdapters(IViewAdapterCollection viewAdapterCollection)
        {
            viewAdapterCollection.RegisterAdapterIfMissing<ContentControlAdapter>();
            viewAdapterCollection.RegisterAdapterIfMissing<PanelAdapter>();
            viewAdapterCollection.RegisterAdapterIfMissing<ItemsControlAdapter>();
            viewAdapterCollection.RegisterAdapterIfMissing<SelectorAdapter>();
        }

        private void RegisterDefaultTypes(ITypeRegistrationService typeService)
{
            typeService.RegisterSingelton<ViewAdapterCollection, IViewAdapterCollection>();
            typeService.RegisterSingeltonMany<ViewProvider>(typeof(IViewRegistration), typeof(IViewProvider));
            typeService.RegisterSingelton<ViewInjectionService, IViewInjectionService>();
            typeService.RegisterInstance<DispatcherQueue>(DispatcherQueue.GetForCurrentThread());
        }

        protected virtual void RegisterViewAdapters(IViewAdapterCollection viewAdapterCollection)
        {
            var adpaters = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(t => typeof(IViewAdapter).IsAssignableFrom(t))
                .Where(t => t.IsClass)
                .Where(t => !t.IsAbstract);

            viewAdapterCollection.RegisterAdapters(adpaters);
        }

        protected virtual void InitializeShell(Window shell)
        {
            Window = shell;
        }

        protected virtual void RegisterViews(IViewRegistration viewRegistration)
        {
            var views = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(t => typeof(IViewFor).IsAssignableFrom(t));

            foreach (var view in views)
            {
                viewRegistration.Register(view);
            }
        }

        protected abstract void InitializeApp();

        public abstract Window CreateShell(IViewProvider viewProvider);


    }
}
