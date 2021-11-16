using System;
using xFrame.Core.MVVM;

namespace xFrame.Core.ViewService
{
    public interface IViewManager
    {
        void AddToContainer(ViewModelBase viewModel, string containerKey);

        void AddToContainer<T>(string containerKey);

        void AddToContainer(Type viewModelType, string containerKey);

        void RemoveFromContainer(ViewModelBase viewModel, string containerKey);
    }
}
