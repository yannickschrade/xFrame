using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.MVVM;

namespace xFrame.Core.ViewInjection
{
    public interface IViewContainer
    {
        object Key { get; }

        IViewAdapterCollection ViewAdapterCollection { get; }

        void AttachTo(object view);

        void RemoveFrom(object view);

        void Inject(Type viewModelType);

        void Inject(IViewModel viewModel);
        void InjectView(object view);
        void InjectView(Type viewType);

        void Remove(Type viewModelType);

        void Remove(IViewModel viewModel);
    }
}
