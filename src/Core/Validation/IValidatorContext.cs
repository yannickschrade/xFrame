using System;

namespace xFrame.Core.Validation
{
    public interface IValidatorContext<T,TProperty>
    {
        IValidatorComponent<T,TProperty> ValidatorComponent{ get; }
        void AddCondition(Func<T,bool> condition);

    }
}
