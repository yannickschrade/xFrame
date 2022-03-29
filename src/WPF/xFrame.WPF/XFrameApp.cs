﻿using System.Reflection;
using System.Windows;
using System;
using xFrame.WPF.ViewAdapters;
using xFrame.WPF.ViewInjection;
using Microsoft.Extensions.DependencyInjection;
using xFrame.Core.ViewInjection;
using System.Linq;
using System.Threading.Tasks;

namespace xFrame.WPF
{
    public class XFrameApp : Application
    {
        public IServiceProvider Services { get; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetupApp();
            InitializeShell(CreateShell());
            await ShowSplashScreen();
            ShowShell();
        }

        public XFrameApp(IServiceProvider serviceProvider)
        {
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
            var shell = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(t => typeof(IShell).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .FirstOrDefault();

            if (shell == null)
                throw new InvalidOperationException("No shell window Found");

            var view = viewProvider.GetView(shell);
            return view is Window window ? window : throw new Exception();
        }

        private async Task ShowSplashScreen()
        {
            var splashVm = Services.GetService<ISplashScreenViewModel>();
            if (splashVm == null)
                return;

            var viewProvider = Services.GetService<IViewProvider>();
            var content = viewProvider.GetViewForViewModel(splashVm);


            var window = new Window();
            if(!string.IsNullOrWhiteSpace(splashVm.StyleKey))
            {
                var style = Resources[splashVm.StyleKey] as Style;
                if(style != null)
                    window.Style = style;
            }
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = content;
            window.Closed += OnShellClosed;
            window.Show();
            await splashVm.LoadAppAsync();
            window.Closed -= OnShellClosed;
            window.Close();
            window = null;
            splashVm = null;
        }

        private void OnShellClosed(object sender, EventArgs e)
        {
            Shutdown();
        }

    }
}