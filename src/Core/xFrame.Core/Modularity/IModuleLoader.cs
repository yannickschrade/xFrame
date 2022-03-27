using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;

namespace xFrame.Core.Modularity
{
    public interface IModuleLoader
    {
        Type ForModuleType { get; }
        IModule CreateModule(IServiceProvider services, Type moduleType);

        void InitializeModule(IServiceProvider services, object module);
    }
}
