using xFrame.Core.IoC;
using xFrame.Core.Modularity;

namespace WPFTestApp.Module1
{
    public class TestModule : IModule
    {
        public string Name => "Test Module";
        public Version Version => new Version(1,0);
        public Exception LoadingException { get; set; }

        public void InitializeModule(ITypeProviderService resolver)
        {
            
        }

        public void RegisterServices(ITypeRegistrationService registrationService)
        {
            
        }
    }
}