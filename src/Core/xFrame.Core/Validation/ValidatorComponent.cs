using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xFrame.Core.Validation
{
    public class ValidatorComponent<T, TProperty> : IValidatorComponent<T, TProperty>
    {
        private string _message;
        private Severity _severity = Severity.Error;
        private Func<T, TProperty, string> _messageFactory;
        private T _typeInstance;
        private TProperty _propertyValue;
        private List<Func<T, bool>> _conditions = new List<Func<T, bool>>();

        public IValidator<TProperty> Validator { get; }

        public ValidatorComponent(T typeInstance, TProperty propertyValue, IValidator<TProperty> validator)
        {
            _typeInstance = typeInstance;
            _propertyValue = propertyValue;
            Validator = validator;
            _message = Validator.DefaultMessage;
        }

        public string GetMessage()
        {
            return _message ?? _messageFactory(_typeInstance, _propertyValue);
        }

        public void SetMessage(string message)
        {
            _message = message;
        }

        public void SetMessage(Func<T, TProperty, string> messageFactory)
        {
            _message = null;
            _messageFactory = messageFactory;
        }

        public void SetSeverity(Severity severity)
        {
            _severity = severity;
        }

        public ValidationMessage Validate(TProperty value)
        {
            if (_conditions.Any(c => !c(_typeInstance)))
                return null;

            if (Validator.IsValid(value))
                return null;


            return new ValidationMessage(GetMessage(), _severity);
        }

        public void AddCondition(Func<T, bool> condtion)
        {
            throw new NotImplementedException();
        }
    }
}
