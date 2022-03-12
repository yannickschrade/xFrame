using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel;

namespace xFrame.Modularity.Abstraction
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IModuleLoader
    {
        Type ForModuleType { get; }
        IModule CreateModule(HostBuilderContext context, IServiceCollection services, Type moduleType);

        void InitializeModule(IServiceProvider services, object module);
    }
}
