using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.WPF.Theming.Generators.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DefaultValueAttribute : Attribute
    {
        public object Value { get; } 
        public DefaultValueAttribute(object value)
        {
            Value = value;
        }
        
    }
}
