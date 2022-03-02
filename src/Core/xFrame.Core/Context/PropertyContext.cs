using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using xFrame.Core.ExtensionMethodes;

namespace xFrame.Core.Context
{
    internal class PropertyContext<T, TProperty> : IPropertyContext<T, TProperty>
    {
        public PropertyInfo Property { get; }
        public T TypeInstance { get; }
        public TProperty PropertyValue => PropertyReader(TypeInstance);
        public Func<T, TProperty> PropertyReader { get; }

        public PropertyContext(Expression<Func<T, TProperty>> expression, T typeInstance)
        {
            TypeInstance = typeInstance;
            PropertyReader = expression.Compile();
            Property = expression.GetPropertyInfo();
        }

        public PropertyContext(Func<T, TProperty> propertyReader, PropertyInfo propertyInfo, T typeInstance)
        {
            TypeInstance = typeInstance;
            Property = propertyInfo;
            PropertyReader = propertyReader;

        }

        public PropertyContext(IPropertyContext<T, TProperty> context)
        {
            Property = context.Property;
            TypeInstance = context.TypeInstance;
            PropertyReader = context.PropertyReader;
        }
    }
}
