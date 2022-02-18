using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xFrame.Core.Fluent;
using xFrame.WPF.Extensions;

namespace xFrame.Core.Validation
{
    internal class PropertyValidationContext<T, TProperty> : PropertyContext<T, TProperty>, IPropertyValidationContext<T, TProperty>
    {
        private readonly List<IValidatorComponent<T, TProperty>> _validators = new();
        private readonly List<Func<T, bool>> _globalConditions = new();
        private readonly List<Action<T, TProperty, ValidationResult>> _validationCallbacks = new();
        private readonly Dictionary<IValidatorComponent<T, TProperty>, List<Func<T, bool>>> _validatorConditions = new();

        public PropertyValidationContext(Expression<Func<T, TProperty>> expression, T classInstance)
            : base(expression, classInstance)
        {
        }

        public PropertyValidationContext(IPropertyContext<T, TProperty> context)
            : base(context)
        {
        }

        public ValidationResult Validate(TProperty property)
        {
            var result = new ValidationResult(Property.Name);

            if (_globalConditions.Any(c => !c(TypeInstance)))
                return result;

            foreach (var validator in _validators)
            {
                if (_validatorConditions.TryGetValue(validator, out var conditions) &&
                    conditions.Any(c => !c(TypeInstance)))
                    return result;

                var component = validator.Validate(PropertyValue);
                if (component != null)
                    result.AddComponent(component);
            }
            Parallel.ForEach(_validationCallbacks, c => c(TypeInstance, PropertyValue, result));
            return result;

        }

        public void AddValidator(IValidator<TProperty> validator)
        {
            var component = new ValidatorComponent<T, TProperty>(TypeInstance, PropertyValue, validator);
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
            var component = new ValidatorComponent<T, TProperty>(TypeInstance, PropertyValue, validator);
            var conditions = _validatorConditions.GetOrSetIfMissing(component);
            conditions.Add(condition);
        }

        public void AddValidationCallBack(Action<T, TProperty, ValidationResult> callback)
        {
            _validationCallbacks.Add(callback);
        }
    }
}
