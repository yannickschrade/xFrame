using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xFrame.Modularity.Abstraction;

namespace xFrame.WPF.Samples.Module1
{
    public class Module : IModule
    {
        public void RegisterServices(IServiceCollection services)
        {
            
        }

        public void OnLoaded(IServiceProvider services)
        {
            services.GetRequiredService<ILogger<Module>>().LogInformation("Module Initialized");
        }
    }
}