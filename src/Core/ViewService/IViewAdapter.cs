using System;
using xFrame.Core.MVVM;

namespace xFrame.Core.ViewService
{
    public interface IViewAdapter
    {
        Type ForType { get; }
        void Add(object view, object container);
        void Remove(object view, object container);

        void NavigateTo(object view, object container);
    }
}
