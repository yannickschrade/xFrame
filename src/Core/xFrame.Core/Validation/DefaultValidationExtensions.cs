using System;
using System.Linq.Expressions;
using xFrame.Core.Commands;
using xFrame.Core.Validators;

namespace xFrame.Core.Validation
{
    public static class DefaultValidationExtensions
    {
        public static IPropertyValidationContext<T, TProperty> NotNull<T, TProperty>(this IPropertyValidationContext<T, TProperty> context,
            Action<IValidatorContext<T, TProperty>> validatorContext)
        {
            var validator = new NotNullValidator<TProperty>();
            var ctx = new ValidatorContext<T, TProperty>(context, validator);
            validatorContext(ctx);
            context.AddValidator(ctx.ValidatorComponent);
            return context;
        }

        public static IPropertyValidationContext<T, TProperty> IsNotNull<T, TProperty>(this IPropertyValidationContext<T, TProperty> context)
        {
            context.AddValidator(new NotNullValidator<TProperty>());
            return context;
        }

        public static IPropertyValidationContext<T, TPoperty> IsNotEmpty<T, TPoperty>(this IPropertyValidationContext<T, TPoperty> context)
        {
            context.AddValidator(new NotEmptyValidator<TPoperty>());
            return context;
        }

        public static IPropertyValidationContext<T, TProperty> IsNotEmpty<T, TProperty>(this IPropertyValidationContext<T, TProperty> context,
            Action<IValidatorContext<T, TProperty>> validatorContext)
        {
            var validator = new NotEmptyValidator<TProperty>();
            var ctx = new ValidatorContext<T,TProperty>(context, validator);
            validatorContext(ctx);
            context.AddValidator(validator);
            return context;
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

        public static IPropertyValidationContext<T, TPoperty> NotifyCommandIfValid<T, TPoperty>(this IPropertyValidationContext<T, TPoperty> context, Expression<Func<T, CommandBase>> command)
        {
            var com = command.Compile()(context.InnerContext.TypeInstance);
            context.IfValid((c, p) => com.RaisCanExecuteChanged());
            return context;
        }

        public static IPropertyValidationContext<T, TProperty> UpdateCommandCanExecute<T, TProperty>(this IPropertyValidationContext<T, TProperty> context, Expression<Func<T, CommandBase>> command)
        {
            var com = command.Compile()(context.InnerContext.TypeInstance);
            context.AddValidationCallBack((c,p,r) => com.RaisCanExecuteChanged(r.IsValid));
            return context;
        }
    }


}
