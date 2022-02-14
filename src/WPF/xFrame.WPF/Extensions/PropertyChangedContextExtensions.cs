using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Threading;
using xFrame.Core.MVVM;

namespace xFrame.WPF.Extensions
{
    public static class PropertyChangedContextExtensions
    {
        public static IPropertyChangedContext<T, TProperty> Execute<T, TProperty>(this IPropertyChangedContext<T, TProperty> context, Action<TProperty> action, Dispatcher dispatcher)
           where T : INotifyPropertyChanged
        {

            context.ExecutionPipline.Add(p => dispatcher.Invoke(action));
            return context;
        }
    }
}
