using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using xFrame.Core.Commands;
using xFrame.Core.ExtensionMethodes;

namespace xFrame.Core.Fluent
{
    public static class PropertyContextExtensions
    {
        public static IPropertyChangedContext<T, TProperty> HasChanged<T, TProperty>(this IPropertyContext<T, TProperty> context)
            where T : INotifyPropertyChanged, new()
        {
            var propertyChangedContext = new PropertyChangedContext<T, TProperty>(context);
            context.ClassInstance.SubscribePropertyChanged(context.Property.Name, async p =>
             {
                 if (p is not TProperty property)
                     throw new InvalidOperationException();

                 propertyChangedContext.PropertyValue = property;

                 foreach (var task in propertyChangedContext.ExecutionPipline)
                 {
                     if (task.Conditions.Any(func => !func(context.ClassInstance)))
                         continue;
                     task.Action(property);
                 }
             });

            return propertyChangedContext;
        }

        public static IPropertyChangedContext<T, TProperty> IF<T, TProperty>(this IPropertyChangedContext<T, TProperty> context, Func<T, bool> condition)
            where T : INotifyPropertyChanged, new()
        {
            context.CurrentExecution.Conditions.Add(condition);
            return context;
        }

        public static IPropertyChangedContext<T, TProperty> Execute<T, TProperty>(this IPropertyChangedContext<T, TProperty> context, Action<TProperty> action)
            where T : INotifyPropertyChanged, new()
        {
            context.ExecutionPipline.Add(new Execution<T, TProperty>(action));
            return context;
        }
        public static IPropertyChangedContext<T, TProperty> NotifyCommand<T, TProperty>(this IPropertyChangedContext<T, TProperty> context, Expression<Func<T, CommandBase>> expression)
            where T : INotifyPropertyChanged, new()
        {
            var commandName = expression.GetPropertyName();
            var command = (CommandBase)context.ClassInstance.GetType().GetProperty(commandName).GetValue(context.ClassInstance);
            context.ExecutionPipline.Add(new Execution<T, TProperty>(P => command.RaisCanExecuteChanged()));
            return context;
        }
    }
}
