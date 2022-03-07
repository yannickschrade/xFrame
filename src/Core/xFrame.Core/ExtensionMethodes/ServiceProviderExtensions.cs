using Microsoft.Extensions.DependencyInjection;
using System;

namespace xFrame.Core.ExtensionMethodes
{
    public static class ServiceProviderExtensions
    {
        public static object GetUnregistredService(this IServiceProvider serviceProvider, Type serviceType)
        {
            return ActivatorUtilities.CreateInstance(serviceProvider, serviceType);
        }

        public static T GetUnregistredService<T>(this IServiceProvider serviceProvider)
        {
            return ActivatorUtilities.CreateInstance<T>(serviceProvider);
        }
    }
}
