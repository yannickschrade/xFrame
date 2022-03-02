using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using xFrame.Core.Commands;
using xFrame.Core.ExtensionMethodes;
using xFrame.Core.Validation;

namespace xFrame.Core.Context
{
    public static class ContextExtensions
    {
        #region PropertyContext

        public static IPropertyContext<T, TProperty> Property<T, TProperty>(this T @class, Expression<Func<T, TProperty>> selector)
        {
            return new PropertyContext<T, TProperty>(selector, @class);
        }

        public static IPropertyContext<T, TProperty> AddChangedHandler<T, TProperty>(this IPropertyContext<T, TProperty> context, Action<IPropertyChangedContext<T, TProperty>> handler)
            where T : INotifyPropertyChanged
        {
            var propertyChangedContext = new PropertyChangedContext<T, TProperty>(context);
            handler(propertyChangedContext);
            context.SubscribePropertyChanged(p =>
            {
                propertyChangedContext.PropertyValue = p;

                foreach (var task in propertyChangedContext.ExecutionPipline)
                {
                    if (task.Conditions.Any(func => !func(context.TypeInstance)))
                        continue;
                    task.Action(p);
                }
            });

            return context;
        }

        public static IPropertyContext<T, TProperty> OnChanged<T, TProperty>(this IPropertyContext<T, TProperty> context, params Action<TProperty>[] callbacks)
            where T : INotifyPropertyChanged
        {

            context.SubscribePropertyChanged(p =>
            {
                foreach (var callback in callbacks)
                {
                    callback(p);
                }
            });
            return context;
        }

        public static IPropertyContext<T, TProperty> OnChanged<T, TProperty>(this IPropertyContext<T, TProperty> context, params Action<T,TProperty>[] callbacks)
            where T : INotifyPropertyChanged
        {

            context.SubscribePropertyChanged(p =>
            {
                foreach (var callback in callbacks)
                {
                    callback(context.TypeInstance,p);
                }
            });
            return context;
        }

        public static IPropertyContext<T, TProperty> OnChanged<T, TProperty>(this IPropertyContext<T, TProperty> context, Action<TProperty> callback)
            where T : INotifyPropertyChanged
        {
            context.SubscribePropertyChanged(p => callback(p));
            return context;
        }

        public static IPropertyContext<T, TProperty> OnChanged<T, TProperty>(this IPropertyContext<T, TProperty> context, Action<T,TProperty> callback)
            where T : INotifyPropertyChanged
        {
            context.SubscribePropertyChanged(p => callback(context.TypeInstance,p));
            return context;
        }

        public static IPropertyContext<T, TProperty> AddValidation<T, TProperty>(this IPropertyContext<T, TProperty> context, Action<IPropertyValidationContext<T, TProperty>> validation)
            where T : IValidatable
        {
            var validationContext = new PropertyValidationContext<T, TProperty>(context);
            validation(validationContext);
            context.SubscribePropertyChanged(p =>
            {
                var result = validationContext.Validate(context.PropertyValue);
                context.TypeInstance.OnValidated(result);
            });

            var result = validationContext.Validate(context.PropertyValue);
            context.TypeInstance.OnValidated(result);
            return context;
        }

        #endregion

        #region PropertyChangedContext

        public static IPropertyChangedContext<T, TProperty> IF<T, TProperty>(this IPropertyChangedContext<T, TProperty> context, Func<T, bool> condition)
           where T : INotifyPropertyChanged
        {
            context.CurrentExecution.Conditions.Add(condition);
            return context;
        }

        public static IPropertyChangedContext<T, TProperty> Execute<T, TProperty>(this IPropertyChangedContext<T, TProperty> context, Action<TProperty> action)
            where T : INotifyPropertyChanged
        {
            context.ExecutionPipline.Add(new Execution<T, TProperty>(action));
            return context;
        }
        public static IPropertyChangedContext<T, TProperty> NotifyCommand<T, TProperty>(this IPropertyChangedContext<T, TProperty> context, Expression<Func<T, TProperty>> expression)
            where TProperty : CommandBase
            where T : INotifyPropertyChanged
        {
            var command = context.PropertyValue;
            context.ExecutionPipline.Add(new Execution<T, TProperty>(P => command.RaisCanExecuteChanged()));
            return context;
        }

        #endregion
    }
}
