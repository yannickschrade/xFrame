using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.Core.MVVM;
using xFrame.Core.ViewInjection;
using xFrame.WPF.Modularity;
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
            var manager = TypeService.Current.Resolve<ModuleManager>();
            RegisterDefaultLoader(manager);
            SetupModuleManager(manager);
            manager.LoadModules();
        }

        private void RegisterDefaultLoader(ModuleManager manager)
        {
            manager.AddModuleLoader<IUiModule>(x =>
            {
                x.Name = "DefaultUILoader";
                x.AddRegistrationPhase();
                x.AddInitialisationPhase();
                x.AddPhase("UIModulePhase", p =>
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
    }
}