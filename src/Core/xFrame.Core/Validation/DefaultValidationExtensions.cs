using System;
using System.Linq.Expressions;
using xFrame.Core.Commands;
using xFrame.Core.Validators;

namespace xFrame.Core.Validation
{
    public static class DefaultValidationExtensions
    {
        public static IPropertyValidationContext<T,TProperty> Transform<T,TProperty,TOld>(this IPropertyValidationContext<T,TOld> context, Func<TOld, TProperty> transformer)
        {
            Func<T, TProperty> expression = T => transformer(context.InnerContext.PropertyReader(T));
            var ctx = new PropertyValidationContext<T,TProperty>(expression,context.InnerContext.Property,context.InnerContext.TypeInstance);
            return ctx;
        }

        public static IValidatorContext<T, TProperty> IsNotNull<T, TProperty>(this IPropertyValidationContext<T, TProperty> context)
        {
            var validator = new NotNullValidator<TProperty>();
            var ctx = new ValidatorContext<T, TProperty>(context, validator);
            context.AddValidator(ctx.ValidatorComponent);
            return ctx;
        }

        public static IValidatorContext<T, TPoperty> IsNotEmpty<T, TPoperty>(this IPropertyValidationContext<T, TPoperty> context)
        {
            var validator = new NotEmptyValidator<TPoperty>();
            var ctx = new ValidatorContext<T,TPoperty>(context, validator);
            context.AddValidator(ctx.ValidatorComponent);
            return ctx;
        }

        public static IPropertyValidationContext<T, TProperty> IfValid<T, TProperty>(this IPropertyValidationContext<T, TProperty> context, Action<T, TProperty> callback)
        {
            context.AddValidationCallBack((@class, property, result) =>
            {
                if (result.IsValid)
                    callback(@class, property);
            });
            return context;
        }


        // TODO FIX!
        //public static IPropertyValidationContext<T, TPoperty> NotifyCommand<T, TPoperty>(this IPropertyValidationContext<T, TPoperty> context, Expression<Func<T, IRelayCommand>> command)
        //{
        //    var com = command.Compile()(context.InnerContext.TypeInstance);
        //    context.AddValidationCallBack((@class, property, result) =>
        //    {
        //        com.RaisCanExecuteChanged(result.IsValid);
        //    });
        //    return context;
        //}

        //public static IPropertyValidationContext<T, TProperty> UpdateCommandCanExecute<T, TProperty>(this IPropertyValidationContext<T, TProperty> context, Expression<Func<T, IRelayCommand>> command)
        //{
        //    var com = command.Compile()(context.InnerContext.TypeInstance);
        //    context.AddValidationCallBack((c,p,r) => com.RaisCanExecuteChanged(r.IsValid));
        //    return context;
        //}
    }


}
