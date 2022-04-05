using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.WPF.Theming.Generators
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class LightenColorAttribute : Attribute
    {
        public int[] Values { get; }
        public LightenColorAttribute(params int[] values)
        {
            Values = values;
        }
    }
}
