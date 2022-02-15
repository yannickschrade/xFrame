using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace xFrame.Core.Fluent
{
    internal class PropertyContext<T, TProperty> : IPropertyContext<T, TProperty>
        where T : new()
    {
        public PropertyInfo Property { get; }
        public T ClassInstance { get; }

        public PropertyContext(PropertyInfo propertyInfo, T classInstance)
        {
            Property = propertyInfo;
            ClassInstance = classInstance;
        }

        public PropertyContext(IPropertyContext<T,TProperty> context)
        {
            Property = context.Property;
            ClassInstance = context.ClassInstance;
        }
    }
}
