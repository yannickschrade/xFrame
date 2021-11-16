using System;
using xFrame.Core.MVVM;
using xFrame.Core.ViewService;

namespace xFrame.WPF.ViewService
{
    public interface IViewProviderService
    {
        IViewFor GetView<T>()
            where T : ViewModelBase;

        IViewFor GetView(Type viewModelType);

        IViewFor GetView(ViewModelBase viewModel);

    }
}