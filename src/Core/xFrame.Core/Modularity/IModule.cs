using System;
using System.Runtime.ExceptionServices;
using xFrame.Core.IoC;

namespace xFrame.Core.Modularity
{
    public interface IModule
    {
        string Name { get; }
        Version Version { get; }
        Exception LoadingException { get; set; }

        void RegisterServices(ITypeRegistrationService registrationService);

        void InitializeModule(ITypeProviderService resolver);
    }
}