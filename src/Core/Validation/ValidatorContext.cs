using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Validation
{
    internal class ValidatorContext<T, TProperty> : IValidatorContext<T, TProperty>
    {
        private IPropertyValidationContext<T, TProperty> _validationContext;
        public IValidatorComponent<T,TProperty> ValidatorComponent { get; }

        public ValidatorContext(IPropertyValidationContext<T, TProperty> context, IValidatorComponent<T,TProperty> validatorComponent)
        {
            _validationContext = context;
            ValidatorComponent = validatorComponent;
        }

        public ValidatorContext(IPropertyValidationContext<T, TProperty> context, IValidator<TProperty> validator)
        {
            _validationContext = context;
            ValidatorComponent = new ValidatorComponent<T,TProperty>(context.TypeInstance, context.PropertyValue, validator);
        }

        public void AddCondition(Func<T, bool> condition)
        {
            _validationContext.AddConditionFor(ValidatorComponent.Validator, condition);
        }
    }
}
