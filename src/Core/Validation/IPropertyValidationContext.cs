using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.Fluent;

namespace xFrame.Core.Validation
{
    public interface IPropertyValidationContext<T, TProperty> : IPropertyContext<T, TProperty>
    {
        ValidationResult Validate(TProperty property);
        void AddValidator(IValidator<TProperty> validator);
        void AddCondition(ICondition condition);
    }
}
