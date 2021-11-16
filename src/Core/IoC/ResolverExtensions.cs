using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xFrame.Core.IoC
{
    public static class ResolverExtensions
    {
        public static T Resolve<T>(this ITypeProviderService resolverService)
        {
            return (T)resolverService.Resolve(typeof(T));
        }
    }
}
