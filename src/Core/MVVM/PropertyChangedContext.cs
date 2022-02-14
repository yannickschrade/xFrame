using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace xFrame.Core.MVVM
{
    internal class PropertyChangedContext<T, Tproperty> : IPropertyChangedContext<T, Tproperty>
        where T : INotifyPropertyChanged
    {
        public List<Action<Tproperty>> ExecutionPipline { get; } = new List<Action<Tproperty>>();
        public Action<Tproperty> CurrentAction => ExecutionPipline.Last();
        public Tproperty Value { get; }
        public T TargetClass { get; }

        public PropertyChangedContext(T targetClass)
        {
            TargetClass = targetClass;
        }
    }
}
