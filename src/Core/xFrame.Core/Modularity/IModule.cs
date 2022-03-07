using System;
using System.Runtime.ExceptionServices;

namespace xFrame.Core.Modularity
{
    public interface IModule
    {
        string Name { get; }
        Version Version { get; }
        Exception LoadingException { get; set; }

        void RegisterServices(IServiceProvider services);

        void InitializeModule(IServiceProvider services);
    }
}