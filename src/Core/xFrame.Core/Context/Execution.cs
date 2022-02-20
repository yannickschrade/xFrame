using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Fluent
{
    public class Execution<T,TProperty>
    {
        public Action<TProperty> Action { get; }

        public List<Func<T, bool>> Conditions { get; } = new List<Func<T, bool>>();

        public Execution(Action<TProperty> action)
        {
            Action = action;
        }
    }
}
