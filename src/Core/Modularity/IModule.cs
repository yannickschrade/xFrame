using xFrame.Core.IoC;

namespace xFrame.Core.Modularity
{
    public interface IModule
    {
        void RegisterServices(ITypeRegistrationService registrationService);

        void OnInitialized(ITypeProviderService resolver);
    }
}