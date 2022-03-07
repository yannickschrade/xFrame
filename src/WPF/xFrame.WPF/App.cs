using System.Reflection;
using System.Windows;
using System;
using xFrame.WPF.ViewAdapters;
using xFrame.WPF.ViewInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using xFrame.Core.ViewInjection;
using System.Linq;

namespace xFrame.WPF
{
    internal class XFrameApp : Application
    {
        private Type _shellViewModelType;
        private bool _stopped;

        public IServiceProvider Services { get; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetupApp();
            InitializeShell(CreateShell());
            ShowShell();
        }

        public XFrameApp(IServiceProvider serviceProvider, Type shellViewModelType)
        {
            _shellViewModelType = shellViewModelType;
            Services = serviceProvider;
        }

        protected virtual void ShowShell()
        {
            MainWindow.Show();
        }

        protected virtual void SetupApp()
        {
            RegisterDefaultViewAdapters(Services.GetRequiredService<IViewAdapterCollection>());
            RegisterViewAdapters(Services.GetRequiredService<IViewAdapterCollection>());
            RegisterViews(Services.GetRequiredService<IViewRegistration>());
        }

        private void RegisterDefaultViewAdapters(IViewAdapterCollection viewAdapterCollection)
        {
            viewAdapterCollection.RegisterAdapterIfMissing<ContentControlAdapter>();
            viewAdapterCollection.RegisterAdapterIfMissing<PanelAdapter>();
            viewAdapterCollection.RegisterAdapterIfMissing<ItemsControlAdapter>();
            viewAdapterCollection.RegisterAdapterIfMissing<SelectorAdapter>();
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

        private Window CreateShell()
        {
            var viewProvider = Services.GetService<IViewProvider>();
            var view = viewProvider.GetViewForViewModel(_shellViewModelType);
            return view is Window window ? window : throw new Exception();
        }

    }
}