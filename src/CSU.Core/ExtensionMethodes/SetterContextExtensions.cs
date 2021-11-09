using CSU.Core.PropertyChanged;
using System.Diagnostics.CodeAnalysis;

namespace CSU.Core.ExtensionMethodes;

public static class SetterContextExtensions
{
    public static SetterContext<T> Notify<T>(this SetterContext<T> context, [NotNull] string propertyName)
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

}
