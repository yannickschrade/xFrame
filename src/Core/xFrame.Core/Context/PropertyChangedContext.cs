using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace xFrame.Core.Context
{
    internal class PropertyChangedContext<T, Tproperty> : IPropertyChangedContext<T, Tproperty>
        where T : INotifyPropertyChanged
    {
        public List<Execution<T, Tproperty>> ExecutionPipline { get; } = new List<Execution<T, Tproperty>>();
        public Execution<T, Tproperty> CurrentExecution => ExecutionPipline.Last();
        public Tproperty PropertyValue { get; set; }
        public IPropertyContext<T, Tproperty> InnerContext { get; }

        public PropertyChangedContext(IPropertyContext<T, Tproperty> context)
        {
            InnerContext = context;
        }
    }
}
