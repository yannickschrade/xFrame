using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace xFrame.Core.Fluent
{
    internal class PropertyChangedContext<T, Tproperty> : PropertyContext<T,Tproperty>, IPropertyChangedContext<T, Tproperty>
        where T : INotifyPropertyChanged, new()
    {
        public List<Execution<T,Tproperty>> ExecutionPipline { get; } = new List<Execution<T,Tproperty>>();
        public Execution<T,Tproperty> CurrentExecution => ExecutionPipline.Last();
        public Tproperty PropertyValue { get; set; }

        public PropertyChangedContext(IPropertyContext<T, Tproperty> context)
            :base(context)
        {
        }
    }
}
