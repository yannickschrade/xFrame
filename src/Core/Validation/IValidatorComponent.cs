using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Validation
{
    public interface IValidatorComponent<T, TProperty>
    {
        IValidator<TProperty> Validator { get; }
        
        void SetMessage(string message);
        void SetMessage(Func<T,TProperty, string> messageFactory); 
        void SetSeverity(Severity severity);
        string GetMessage();
        void AddCondition(Func<T, bool> condtion);
        ValidationMessage Validate(TProperty value);
    }
}
