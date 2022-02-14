using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Xsl;
using xFrame.Core.Commands;
using xFrame.Core.ExtensionMethodes;

namespace xFrame.Core.MVVM
{
    public static class PropertyChangedContextExtensions
    {
        public static IPropertyChangedContext<T, TProperty> Execute<T, TProperty>(this IPropertyChangedContext<T, TProperty> context, Action<TProperty> action)
            where T : INotifyPropertyChanged
        {
            context.ExecutionPipline.Add(action);
            return context;
        }
        public static IPropertyChangedContext<T, TProperty> NotifyCommand<T,TProperty>(this IPropertyChangedContext<T,TProperty> context, Expression<Func<T, CommandBase>> expression)
        {
            var commandName = expression.GetMebmerName();
            var command = (CommandBase)context.TargetClass.GetType().GetProperty(commandName).GetValue(context.TargetClass);
            context.ExecutionPipline.Add(p => command.RaisCanExecuteChanged());
            return context;


        }
    }
}
