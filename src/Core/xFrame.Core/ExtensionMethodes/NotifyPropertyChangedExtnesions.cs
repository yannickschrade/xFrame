using System;
using System.ComponentModel;
using System.Linq.Expressions;
using xFrame.Core.Context;

namespace xFrame.Core.ExtensionMethodes
{
    public static class NotifyPropertyChangedExtnesions
    {
        public static void SubscribePropertyChanged<T,TProperty>(this T notifier, Expression<Func<T,TProperty>> property, Action<TProperty> callback)
            where T : INotifyPropertyChanged
        {
            var name = property.GetPropertyName();
            var reader = property.Compile();
            notifier.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == name)
                {
                    
                    callback(reader(notifier));
                }
            };
        }

        internal static void SubscribePropertyChanged<T,TProperty>(this IPropertyContext<T,TProperty> context, Action<TProperty> callback)
            where T : INotifyPropertyChanged
        {
            var name = context.Property.Name;
            context.TypeInstance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == name)
                {

                    callback(context.PropertyReader(context.TypeInstance));
                }
            };
        }
    }
}
