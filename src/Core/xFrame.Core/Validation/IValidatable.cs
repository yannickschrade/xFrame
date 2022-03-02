using System.ComponentModel;

namespace xFrame.Core.Validation
{
    public interface IValidatable : INotifyPropertyChanged
    {
        void OnValidated(ValidationResult result);
    }
}
