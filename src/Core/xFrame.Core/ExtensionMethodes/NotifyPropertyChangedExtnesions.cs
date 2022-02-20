using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace xFrame.Core.ExtensionMethodes
{
    public static class NotifyPropertyChangedExtnesions
    {
        public static void SubscribePropertyChanged<T,TProperty>(this T notifier, Expression<Func<T,TProperty>> property, Action<TProperty> action)
            where T : INotifyPropertyChanged
        {
            var name = property.GetPropertyName();
            var reader = property.Compile();
            notifier.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == name)
                {
                    
                    action(reader(notifier));
                }
            };
        }
    }
}
