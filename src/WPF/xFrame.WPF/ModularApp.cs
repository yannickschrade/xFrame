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
               x.Name("DefaultUILoader")
               .AddRegistrationPhase()
               .AddPhase(p =>
               p.Name("Load UIModules")
               .AddLoadingAction(a => 
               a.AddExecute(m => m.SetupViews(TypeService.Current.Resolve<IViewInjectionService>())))));
        }
    }
}