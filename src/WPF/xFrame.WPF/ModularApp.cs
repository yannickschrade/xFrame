using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.Core.MVVM;
using xFrame.Core.ViewInjection;
using xFrame.WPF.Modularity;
using xFrame.WPF.ViewInjection;

namespace xFrame.WPF
{
    public abstract class ModularApp<T> : App<T>
        where T : IViewModel
    {
        protected abstract override void RegisterTypes(ITypeRegistrationService typeRegistration);

        protected abstract void SetupModuleManager(ModuleManager moduleManager);

        protected override void SetupApp()
        {
            base.SetupApp();
            var manager = new ModuleManager();
            SetupModuleManager(manager);
            manager.LoadModules();
        }

        private void RegisterViews(IUiModule module)
        {
            var assembly = Assembly.GetAssembly(module.GetType());
            var views = assembly.GetTypes()
                    .Where(t => typeof(IViewFor).IsAssignableFrom(t));

            var viewRegistration = TypeService.Resolve<IViewRegistration>();
            foreach (var view in views)
            {
                viewRegistration.Register(view);
            }
        }

        private void RegisterServices(IModule module)
        {
            module.RegisterServices(TypeService);
        }

        private void InitializeModule(IModule module)
        {
            module.InitializeModule(TypeService);
        }

        private void SetupViews(IUiModule uiModule)
        {
            var viewInjectionService = TypeService.Resolve<IViewInjectionService>();
            uiModule.SetupViews(viewInjectionService);
        }
    }
}