using System;

namespace xFrame.Core.IoC
{
    public static class RegistrationExtensions
    {
        public static ITypeRegistrationService RegisterType<Tfrom, Tto>(this ITypeRegistrationService registrationService)
        {
            return registrationService.RegisterType(typeof(Tfrom), typeof(Tto));
        }

        public static ITypeRegistrationService RegisterSingelton<Tfrom, Tto>(this ITypeRegistrationService registrationService)
        {
            return registrationService.RegisterSingelton(typeof(Tfrom), typeof(Tto));
        }

        public static ITypeRegistrationService RegisterInstance<T>(this ITypeRegistrationService registrationService, object instance)
        {
            return registrationService.RegisterInstance(typeof(T), instance);
        }

        public static ITypeRegistrationService RegisterMany<T>(this ITypeRegistrationService registrationService, params Type[] serviceTypes)
        {
            return registrationService.RegisterMany(typeof(T), serviceTypes);
        }

        public static ITypeRegistrationService RegisterSingeltonMany<T>(this ITypeRegistrationService registrationService, params Type[] serviceTypes)
        {
            return registrationService.RegisterSingeltonMany(typeof(T), serviceTypes);
        }
    }
}
