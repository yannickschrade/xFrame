using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.Validators;

namespace xFrame.Core.Validation
{
    public static class DefaultValidationExtensions
    {
        public static IPropertyValidationContext<T, TProperty> NotNull<T,TProperty>(this IPropertyValidationContext<T,TProperty> context)
        {
            context.AddValidator(new NotNullValidator<TProperty>());
            return context;
        }
    }
}
