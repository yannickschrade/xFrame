using System;
using System.Collections.Generic;
using xFrame.Core.Commands;

namespace xFrame.Core.MVVM
{
    public interface IDialogViewModel : IViewModel
    {
        IEnumerable<DialogButton> DialogButtons { get; }
        Action CloseDialogAction { get; set; }
        bool? Confirmed { get; set; }

    }
}
