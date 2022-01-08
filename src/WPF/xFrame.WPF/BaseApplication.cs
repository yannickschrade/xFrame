using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.Core.MVVM;
using xFrame.Core.ViewInjection;
using xFrame.WPF.ViewAdapters;
using xFrame.WPF.ViewInjection;

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
        }

        private void RegisterDefaultTypes(ITypeRegistrationService typeService)
        {
            typeService.RegisterSingelton<ViewAdapterCollection, IViewAdapterCollection>();
            typeService.RegisterSingeltonMany<ViewProvider>(typeof(IViewRegistration), typeof(IViewProvider));
            typeService.RegisterSingelton<ViewInjectionService, IViewInjectionService>();
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
            MainWindow = shell;
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

        protected abstract void OnInitialized();

        private Window CreateShell()
        {
            var viewProvider = TypeService.Resolve<IViewProvider>();
            var view = viewProvider.GetViewForViewModel<T>();
            return view is Window window ? window : throw new Exception();
        }
    }
}