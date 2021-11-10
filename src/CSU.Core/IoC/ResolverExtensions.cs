using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSU.Core.IoC
{
    public static class ResolverExtensions
    {
        public static T Resolve<T>(this IResolverContainer container)
        {
            return (T)Convert.ChangeType(container.Resolve(typeof(T)), typeof(T));
        }
    }
}
