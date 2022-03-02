using System;

namespace xFrame.Core.Validation
{
    public interface IValidatorContext<T,TProperty>
    {
        IValidatorComponent<T,TProperty> ValidatorComponent{ get; }
        void AddCondition(Func<T,bool> condition);
        void WithMessage(string message);
        void WithMessage(Func<T, TProperty, string> messageFactory);
        void WithSeverity(Severity severity);

    }
}
