using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Modularity.Abstraction;

namespace xFrame.WPF.Modularity
{
    public static class HostModuleExtensions
    {
        public static IHost LoadModules(this IHost host)
        {
            var manager = host.Services.GetRequiredService<IModuleManager>();
            manager.InitializeModules();

            return host;
        }
    }
}
