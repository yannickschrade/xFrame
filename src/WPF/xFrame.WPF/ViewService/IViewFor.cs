using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewService;

public interface IViewFor<T>
    where T : ViewModelBase
{
    public T? DataContext { get; set; }
}
