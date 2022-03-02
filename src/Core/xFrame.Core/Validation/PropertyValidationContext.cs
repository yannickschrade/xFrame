using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xFrame.Core.Context;
using xFrame.Core.ExtensionMethodes;

namespace xFrame.Core.Validation
{
    internal class PropertyValidationContext<T, TProperty> : IPropertyValidationContext<T, TProperty>
    {
        private readonly List<IValidatorComponent<T, TProperty>> _validators = new();
        private readonly List<Func<T, bool>> _globalConditions = new();
        private readonly List<Action<T, TProperty, ValidationResult>> _validationCallbacks = new();
        private readonly Dictionary<IValidatorComponent<T, TProperty>, List<Func<T, bool>>> _validatorConditions = new();

        public IPropertyContext<T, TProperty> InnerContext { get; }

        public PropertyValidationContext(Expression<Func<T, TProperty>> expression, T classInstance)
        {
        }

        public PropertyValidationContext(IPropertyContext<T, TProperty> context)
        {
            InnerContext = context;
        }

        public ValidationResult Validate(TProperty property)
        {
            var result = new ValidationResult(InnerContext.Property.Name);

            if (_globalConditions.Any(c => !c(InnerContext.TypeInstance)))
                return result;

            foreach (var validator in _validators)
            {
                if (_validatorConditions.TryGetValue(validator, out var conditions) &&
                    conditions.Any(c => !c(InnerContext.TypeInstance)))
                    return result;

                var component = validator.Validate(InnerContext.PropertyValue);
                if (component != null)
                    result.AddComponent(component);
            }
            Parallel.ForEach(_validationCallbacks, c => c(InnerContext.TypeInstance, InnerContext.PropertyValue, result));
            return result;

        }

        public void AddValidator(IValidator<TProperty> validator)
        {
            var component = new ValidatorComponent<T, TProperty>(InnerContext.TypeInstance, InnerContext.PropertyValue, validator);
            _validators.Add(component);
        }

        public void AddValidator(IValidatorComponent<T, TProperty> component)
        {
            _validators.Add(component);
        }

        public void AddCondition(Func<T, bool> condition)
        {
            _globalConditions.Add(condition);
        }

        public void AddConditionFor(IValidator<TProperty> validator, Func<T, bool> condition)
        {
            var component = new ValidatorComponent<T, TProperty>(InnerContext.TypeInstance, InnerContext.PropertyValue, validator);
            var conditions = _validatorConditions.GetOrSetIfMissing(component);
            conditions.Add(condition);
        }

        public void AddValidationCallBack(Action<T, TProperty, ValidationResult> callback)
        {
            _validationCallbacks.Add(callback);
        }
    }
}
