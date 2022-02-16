using System.Collections.Generic;

namespace xFrame.Core.Validation
{
    public interface IValidator<T,TProperty>
    {
        string Name { get; }
        IEnumerable<string> ErrorMessages { get; }
        ValidationFailure Run(IValidationContext<T> context,TProperty property);

    }
}