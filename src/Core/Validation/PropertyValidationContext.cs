using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using xFrame.Core.Fluent;
using xFrame.WPF.Extensions;

namespace xFrame.Core.Validation
{
    internal class PropertyValidationContext<T, TProperty> : PropertyContext<T,TProperty>, IPropertyValidationContext<T, TProperty>
    {
        private List<IValidator<TProperty>> _validators;
        private List<ICondition> _globalConditions;
        private Dictionary<IValidator<TProperty>, List<ICondition>> _validatorConditions;

        public PropertyValidationContext(Expression<Func<T,TProperty>> expression, T classInstance) 
            : base(expression, classInstance)
        {
        }

        public PropertyValidationContext(IPropertyContext<T,TProperty> context)
            : base(context)
        {

        }

        public ValidationResult Validate(TProperty property)
        {
            if (_globalConditions.Any(c => !c.IsFulfilled()))
                return ValidationResult.ValidResult(Property.Name);

            var results = new List<ValidationResult>();
            foreach (var validator in _validators)
            {
                if(_validatorConditions[validator]?.Any(c => !c.IsFulfilled()) == true)
                    return ValidationResult.ValidResult(Property.Name);
                results.Add(validator.Run(Value));
            }

            return ValidationResult.FromFailures(results);
                
        }

        public void AddValidator(IValidator<TProperty> validator)
        {
            _validators.Add(validator);
        }

        public void AddCondition(ICondition condition)
        {
            _globalConditions.Add(condition);
        }

        public void AddConditionFor(IValidator<TProperty> validator, ICondition condition)
        {
            var conditions = _validatorConditions.GetOrSetIfMissing(validator);
            conditions.Add(condition);
        }
    }
}
