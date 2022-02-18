using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.Validation;

namespace xFrame.Core.Validators
{
    public class NotNullValidator<TProperty> : IValidator<TProperty>
    {
        public string Name => "Not Null Validator";

        public string DefaultMessage => "value can't be null";

        public  bool IsValid(TProperty value)
            => value != null;
    }
}
