using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.Validation;

namespace xFrame.Core.Validators
{
    public class NotNullValidator<T,TProperty> : Validator<T,TProperty>
    {
        public override string Name => "Not Null Validator";

        protected override ValidationFailure IsValid(TProperty property)
            => ValidationFailure.FromCondition("Test",() => property == null, "Value can't be null");
    }
}
