using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSU.Core.PropertyChanged
{
    public readonly ref struct SetterContext<T>
    {

        public string PropertyName { get; }
        public bool HasChanged { get; }
        public T Value { get; }
        public Action<string> OnPropertyChanged { get; }

        public SetterContext(string propertyName, bool hasChanged, T value, Action<string> onPropertyChanged)
        {
            PropertyName = propertyName;
            HasChanged = hasChanged;
            Value = value;
            OnPropertyChanged = onPropertyChanged;
        }
    }
}
