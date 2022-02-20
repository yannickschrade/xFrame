using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.Validation;

namespace xFrame.Core.Validators
{
    public class NotEmptyValidator<TPoperty> : IValidator<TPoperty>
    {
        public string Name => "Not empty validator";
        public string DefaultMessage => "can't be empty";

        public bool IsValid(TPoperty value)
        {
            switch (value)
            {
                case null:
                case string s when string.IsNullOrWhiteSpace(s):
                case ICollection { Count: 0 }:
                case Array { Length: 0 }:
                case IEnumerable e when !e.GetEnumerator().MoveNext():
                    return false;
            }
            return true;
        }
    }
}
