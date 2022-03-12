using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Modularity.Abstraction
{
    public interface IModule
    {
        void RegisterServices(IServiceCollection services);

        void OnLoaded(IServiceProvider services);
    }
}
