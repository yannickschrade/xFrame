using System.ComponentModel;
using xFrame.Core.Validation;

namespace xFrame.Core.MVVM
{
    public interface IViewModel : INotifyPropertyChanged, INotifyDataErrorInfo, IValidatable
    {
        void OnViewStateChanged(bool IsActive);

    }
}