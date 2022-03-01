using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.ViewInjection;

namespace xFrame.Core.MVVM
{
    public interface IDialogViewModel
    {
        bool CanClose();
        DialogResult OnClose();
    }
}
