using System.Linq;
using System.Reflection;
using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.Core.MVVM;
using xFrame.Core.ViewInjection;
using xFrame.WPF.Modularity;
using xFrame.WPF.ViewInjection;

namespace xFrame.WPF
{
    public abstract class ModularApp : App
    {
        protected abstract override void RegisterTypes(ITypeRegistrationService typeRegistration);

        protected abstract void SetupModuleManager(ModuleManager moduleManager);

        protected override void SetupApp()
        {
            base.SetupApp();
            var manager = TypeService.Current.Resolve<ModuleManager>();
            RegisterDefaultLoader(manager);
            SetupModuleManager(manager);
            manager.LoadModules();
        }

        private void RegisterDefaultLoader(ModuleManager manager)
        {
            manager.AddModuleLoader<IUiModule>(x =>
            {
                x.Name = "DefaultUIModuleLoader";
                x.AddPhase(DefaultLoadingPhase.TypeRegistration,x =>
                {
                    x.AddLoadingAction(m => m.RegisterServices(TypeService.Current));
                    x.AddLoadingAction(m => DiscoverControls(m));
                });
                x.AddInitialisationPhase();
                x.AddPhase("UIModuleInit", p =>
                {
                    p.Name = "Setup UI";
                    p.AddLoadingAction(m => m.SetupViews(TypeService.Current.Resolve<IViewInjectionService>()));
                });
            });

            manager.AddModuleLoader<IModule>(x =>
            {
                x.Name = "Default Module Loader";
                x.AddRegistrationPhase();
                x.AddInitialisationPhase();
            });
        }

        private void DiscoverControls(IUiModule module)
        {
            var viewRegistration = TypeService.Current.Resolve<IViewRegistration>();
            var views = Assembly.GetAssembly(module.GetType())
                .GetTypes()
                .Where(t => typeof(IViewFor).IsAssignableFrom(t));

            foreach (var view in views)
            {
                viewRegistration.Register(view);
            }
        }
    }
}