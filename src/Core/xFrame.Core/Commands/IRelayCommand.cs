using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace xFrame.Core.Commands
{
    public interface IRelayCommand : ICommand
    {
        void NotifyCanExecuteChanged();

    }

    public interface IRelayCommand<in T> : IRelayCommand
    {
        bool CanExecute(T parameter);
        void Execute(T parameter);
    }
}
