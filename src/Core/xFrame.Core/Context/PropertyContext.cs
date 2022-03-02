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
        public Expression<Func<T, TProperty>> Expression { get; }
        public Func<T, TProperty> PropertyReader { get; }

        public PropertyContext(Expression<Func<T, TProperty>> expression, T classInstance)
        {
            Expression = expression;
            TypeInstance = classInstance;

            PropertyReader = expression.Compile();
            Property = expression.GetPropertyInfo();
        }

        public PropertyContext(IPropertyContext<T, TProperty> context)
        {
            Property = context.Property;
            TypeInstance = context.TypeInstance;
            Expression = context.Expression;
            PropertyReader = context.PropertyReader;
        }
    }
}
