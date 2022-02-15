using System.Windows;
using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.WPF;

namespace WPFTestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : ModularApp<ViewModel>
    {
        protected override void OnInitialized()
        {

        }

        protected override void RegisterTypes(ITypeRegistrationService typeRegistration)
        {

        }

        protected override void SetupModuleManager(ModuleManager moduleManager)
        {
            moduleManager.AddModuleLoader<IModule>(x =>
            {
                x.Name("DefaultLoader")
                .AddRegistrationPhase();
            });
        }
    }
}
