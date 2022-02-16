using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.Validation;

namespace xFrame.Core.Validators
{
    public abstract class Validator<T> : IValidator<T, TPoperty>
    {
        public abstract string Name { get; }
        public IEnumerable<string> ErrorMessages { get; }

        public ValidationFailure Run(T property)
        {
            return IsValid(property);
        }

        protected abstract ValidationFailure IsValid(T property);
    }
}
