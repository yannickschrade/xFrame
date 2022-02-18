using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Validation
{
    public static class DefaultConditions
    {
        public static IValidatorContext<T,TProperty> When<T,TProperty>(this IValidatorContext<T,TProperty> context, Func<T,bool> condition)
        {
            context.AddCondition(condition);
            return context;
        }
    }
}
