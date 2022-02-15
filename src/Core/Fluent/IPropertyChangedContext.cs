using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace xFrame.Core.Fluent
{
    public interface IPropertyChangedContext<T, TProperty> : IPropertyContext<T, TProperty>
        where T : INotifyPropertyChanged, new()
    {

        List<Execution<T,TProperty>> ExecutionPipline { get; }
        Execution<T,TProperty> CurrentExecution { get; }
        TProperty PropertyValue { get; }
    }
}
