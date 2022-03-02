using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using xFrame.Core.Commands;
using xFrame.Core.ViewInjection;

namespace xFrame.Core.MVVM
{
    public interface IDialogViewModel : IViewModel
    {
        RelayCommand<bool> CloseDialogCommand { get; } 
        void OnLoaded();
    }
}
