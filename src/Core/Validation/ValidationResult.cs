using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace xFrame.Core.Validation
{
    public class ValidationResult
    {
        private IEnumerable<ValidationFailure> _failures;
        public IEnumerable<string> ValidatedPropertys { get; }
        public bool IsValid => _failures?.Any() != true;

        public IEnumerable<string> ErrorMessages => _failures.Select(f => f.ErrorMessage);

        public ValidationResult(string validatedProperty)
        {
            ValidatedPropertys = new List<string> { validatedProperty };
        }


        public ValidationResult(IEnumerable<ValidationFailure> failures)
        {
            _failures = failures;
        }

        public static ValidationResult ValidResult(string validatedProperty)
        {
            return new ValidationResult(validatedProperty);
        }
    }
}