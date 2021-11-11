using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
