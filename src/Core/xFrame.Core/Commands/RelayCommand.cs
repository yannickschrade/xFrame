using System;
using System.Windows.Input;

namespace xFrame.Core.Commands
{
    public class RelayCommand : CommandBase
    {

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) : base(execute, canExecute)
        {
        }

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
                ExecuteAction(parameter);
        }
    }


    public class RelayCommand<T> : RelayCommand
    {
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null) 
            : base(p => execute((T)Convert.ChangeType(p, typeof(T))), canExecute != null ? p => canExecute((T)p) : null)
        {
        }
    }
}