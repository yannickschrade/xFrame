using System;
using xFrame.Core.Commands;
using xFrame.Core.PropertyChanged;

namespace xFrame.Core.ExtensionMethodes
{
    public static class SetterContextExtensions
    {
        public static SetterContext<T> Notify<T>(this SetterContext<T> context, string propertyName)
        {
            if (context.HasChanged)
            {
                context.OnPropertyChanged(propertyName);
            }
            return context;
        }

        public static SetterContext<T> NotifyMany<T>(this SetterContext<T> context, params string[] propertyNames)
        {
            if (context.HasChanged)
            {
                foreach (var propertyName in propertyNames)
                {
                    context.OnPropertyChanged(propertyName);
                }
            }
            return context;
        }

        public static SetterContext<T> ExecuteOnUpdate<T>(this SetterContext<T> context, Action<T> action)
        {
            if (context.HasChanged)
            {
                action(context.Value);
            }
            return context;
        }

        public static SetterContext<T> NotifyCommand<T>(this SetterContext<T> context, BaseCommand command)
        {
            if (context.HasChanged)
            {
                command.RaisCanExecuteChanged();
            }
            return context;
        }
    }
}