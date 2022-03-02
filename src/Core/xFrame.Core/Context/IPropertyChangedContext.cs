using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace xFrame.Core.Context
{
    public interface IPropertyChangedContext<T, TProperty>
        where T : INotifyPropertyChanged
    {
        IPropertyContext<T, TProperty> InnerContext { get; }
        List<Execution<T, TProperty>> ExecutionPipline { get; }
        Execution<T, TProperty> CurrentExecution { get; }
        TProperty PropertyValue { get; }
    }
}
