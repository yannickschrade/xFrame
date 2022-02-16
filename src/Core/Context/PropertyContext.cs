using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using xFrame.Core.ExtensionMethodes;

namespace xFrame.Core.Fluent
{
    internal class PropertyContext<T, TProperty> : IPropertyContext<T, TProperty>
    {
        private Func<T, TProperty> propertyReader; 
        public PropertyInfo Property { get; }
        public T TypeInstance { get; }
        public TProperty Value => propertyReader(TypeInstance);
        public Expression<Func<T, TProperty>> Expression { get; }


        public PropertyContext(Expression<Func<T,TProperty>> expression, T classInstance)
        {
            Property = expression.GetPropertyInfo();
            TypeInstance = classInstance;
        }

        public PropertyContext(IPropertyContext<T,TProperty> context)
        {
            Property = context.Property;
            TypeInstance = context.TypeInstance;
        }
    }
}
