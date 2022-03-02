using System;
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
            ValidatorComponent = new ValidatorComponent<T,TProperty>(context.InnerContext.TypeInstance, context.InnerContext.PropertyValue, validator);
        }

        public void AddCondition(Func<T, bool> condition)
        {
            _validationContext.AddConditionFor(ValidatorComponent.Validator, condition);
        }

        public void WithMessage(string message)
        {
            ValidatorComponent.SetMessage(message);
        }

        public void WithMessage(Func<T, TProperty, string> messageFactory)
        {
            ValidatorComponent.SetMessage(messageFactory);
        }

        public void WithSeverity(Severity severity)
        {
            ValidatorComponent.SetSeverity(severity);
        }
    }
}
