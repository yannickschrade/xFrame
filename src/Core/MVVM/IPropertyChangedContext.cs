using System;
using System.Collections.Generic;

namespace xFrame.Core.MVVM
{
    public interface IPropertyChangedContext<T, Tproperty>
    {
        public T TargetClass { get; }
        public List<Action<Tproperty>> ExecutionPipline { get; }


        public Action<Tproperty> CurrentAction { get; }

        public Tproperty Value { get; }
    }
}
