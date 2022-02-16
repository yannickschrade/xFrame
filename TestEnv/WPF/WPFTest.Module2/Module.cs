using System;
 using xFrame.Core.IoC;
using xFrame.Core.ViewInjection;
using xFrame.WPF.Modularity;

namespace WPFTest.Module2
{
    public class Module : IUiModule
    {
        public string Name => "Test Module 2";
        public Version Version => new Version(1, 0);
        public Exception LoadingException { get; set; }

        public void InitializeModule(ITypeProviderService resolver)
        {
            
        }

        public void RegisterServices(ITypeRegistrationService registrationService)
        {
            
        }

        public void SetupViews(IViewInjectionService viewInjectionService)
        {
            
        }
    }
}
