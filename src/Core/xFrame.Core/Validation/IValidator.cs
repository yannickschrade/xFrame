using System.Collections.Generic;

namespace xFrame.Core.Validation
{
    public interface IValidator<TProperty>
    {
        string Name { get; }
        string DefaultMessage { get; }
        bool IsValid(TProperty value);

    }
}