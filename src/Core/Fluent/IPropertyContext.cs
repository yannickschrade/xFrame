using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace xFrame.Core.Fluent
{
    public interface IPropertyContext<T, TProperty>
        where T : new()
    {
        PropertyInfo Property { get; }

        T ClassInstance { get; }

    }
}
