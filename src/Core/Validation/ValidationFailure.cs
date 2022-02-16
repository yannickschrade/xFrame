using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Validation
{
    public class ValidationFailure
    {
        public string PropertyName { get; private set; }
        public string ErrorMessage { get; private set; }


        public static ValidationFailure FromCondition(string propertyName,Func<bool> failingCondition, string message)
        {
            ValidationFailure failure = null;

            if (failingCondition())
            {
                failure = new ValidationFailure
                {
                    ErrorMessage = message,
                    PropertyName = propertyName
                };
            }

            return failure;
        }
    }
}
