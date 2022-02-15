using System.Windows;
using WPFTest.Module2;
using WPFTestApp.Module1;
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
            moduleManager.AddModule<TestModule>();
            moduleManager.AddModule<Module>();
        }
    }
}
