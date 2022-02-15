using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace xFrame.Core.IoC
{
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public static class TypeService
    {
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public static ITypeService Current { get; set; }
    }
}
