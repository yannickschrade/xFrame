using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.IoC;

namespace xFrame.WPF.ViewInjection
{
    internal static class TypeProvider
    {
        public static ITypeProviderService Current { get;set; }
    }
}
