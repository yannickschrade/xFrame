using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewInjection
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IViewFor
    {
        Type ViewModelType { get; }
    }

    public interface IViewFor<T> : IViewFor
        where T : class, IViewModel
    {
        Type IViewFor.ViewModelType => typeof(T);
    }


}
