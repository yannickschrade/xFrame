using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewInjection
{
    public interface IViewFor
    {
        Type ViewModelType { get; }
    }

    public interface IViewFor<T> : IViewFor
        where T : ViewModelBase
    {
        Type IViewFor.ViewModelType => typeof(T);
    }


}
