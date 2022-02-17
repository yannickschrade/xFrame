using System.Collections.Generic;
using System.Linq;

namespace xFrame.Core.Validation
{
    public class ValidationResult
    {
        private List<ValidationMessage> _messages = new ();
        public IEnumerable<ValidationMessage> Messages => _messages;

        public string ValidatedProperty { get; }

        public bool IsValid => Messages?.Any(f => f.Severity == Severity.Error) != true;

        public ValidationResult(string validatedProperty)
        {
            ValidatedProperty = validatedProperty;
        }

        internal void AddComponent(ValidationMessage failure)
        {
            _messages.Add(failure);
        }

        public static ValidationResult ValidResult(string validatedProperty)
        {
            return new ValidationResult(validatedProperty);
        }
    }
}