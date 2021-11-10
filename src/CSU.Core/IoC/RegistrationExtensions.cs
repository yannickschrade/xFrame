using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSU.Core.IoC
{
    public static class RegistrationExtensions
    {
        public static IRegistrationContainer RegisterType<Tfrom, Tto>(this IRegistrationContainer container)
        {
            return container.RegisterType(typeof(Tfrom), typeof(Tto));
        }

        public static IRegistrationContainer RegisterSingelton<Tfrom, Tto>(this IRegistrationContainer container)
        {
            return container.RegisterSingelton(typeof(Tfrom), typeof(Tto));
        }
    }
}
