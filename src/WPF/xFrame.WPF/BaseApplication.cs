using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.Core.MVVM;
using xFrame.Core.ViewService;
using xFrame.WPF.ViewAdapters;
using xFrame.WPF.ViewInjection;
using xFrame.WPF.ViewService;

namespace xFrame.WPF
{
    public abstract class BaseApplication<T> : Application
        where T : ViewModelBase

    {
        protected ITypeService TypeService;

        protected abstract ITypeService CreateTypeService();
        protected abstract void RegisterTypes(ITypeRegistrationService typeRegistration);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetupApp();
            InitializeShell(CreateShell());
            OnInitialized();
            ShowShell();
        }

        protected virtual void ShowShell()
        {
            MainWindow.Show();
        }

        protected virtual void SetupApp()
        {
            TypeService = CreateTypeService();
            RegisterDefaultTypes(TypeService);
            RegisterTypes(TypeService);
            RegisterDefaultViewAdapters(TypeService.Resolve<IViewAdapterCollection>());
            RegisterViewAdapters(TypeService.Resolve<IViewAdapterCollection>());
            RegisterViews(TypeService.Resolve<IViewRegistrationService>());
        }

        private void RegisterDefaultViewAdapters(IViewAdapterCollection viewAdapterCollection)
        {
            viewAdapterCollection.RegisterAdapterIfMissing<ContentControlAdapter>();
            viewAdapterCollection.RegisterAdapterIfMissing<ItemsControlAdapter>();
            viewAdapterCollection.RegisterAdapterIfMissing<PanelAdapter>();
        }

        private void RegisterDefaultTypes(ITypeRegistrationService typeService)
        {
            typeService.RegisterSingelton<ViewAdapterCollection, IViewAdapterCollection>();
            typeService.RegisterSingelton<ViewManager, IViewManager>();
            typeService.RegisterSingeltonMany<ViewRegistry>(typeof(IViewProviderService), typeof(IViewRegistrationService));
        }

        protected virtual void RegisterViewAdapters(IViewAdapterCollection viewAdapterCollection)
        {
            var adpaters = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(t => typeof(IViewAdapter).IsAssignableFrom(t))
                .Where(t => t.IsClass)
                .Where(t => !t.IsAbstract);

            viewAdapterCollection.RegisterAdaptersIfMissing(adpaters);
        }

        protected virtual void InitializeShell(Window shell)
        {
            MainWindow = shell;
        }

        protected virtual void RegisterViews(IViewRegistrationService viewRegistrationService)
        {
            var views = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(t => typeof(IViewFor).IsAssignableFrom(t));

            foreach (var view in views)
            {
                viewRegistrationService.RegisterView(view);
            }
        }

        protected abstract void OnInitialized();

        private Window CreateShell()
        {
            var viewProvider = TypeService.Resolve<IViewProviderService>();
            var view = viewProvider.GetView<T>();
            if(view is Window window)
            {
                return window;
            }

            throw new Exception();
        }
    }
}