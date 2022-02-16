using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace xFrame.Core.Fluent
{
    public interface IPropertyContext<T, TProperty>
    {
        PropertyInfo Property { get; }

        T TypeInstance { get; }

        TProperty Value { get; }
        Expression<Func<T, TProperty>> Expression { get;}
    }
}
