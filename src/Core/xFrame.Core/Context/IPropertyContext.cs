using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace xFrame.Core.Context
{
    public interface IPropertyContext<T, TProperty>
    {
        PropertyInfo Property { get; }
        T TypeInstance { get; }
        TProperty PropertyValue { get; }
        Expression<Func<T, TProperty>> Expression { get; }
        Func<T, TProperty> PropertyReader { get; }
    }
}
