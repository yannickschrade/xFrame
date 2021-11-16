using System;
using System.Windows.Input;

namespace xFrame.Core.Commands
{
    public class RelayCommand : BaseCommand
    {

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null) : base(execute, canExecute)
        {
        }

        public override bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc(parameter);
        }

        public override void Execute(object parameter)
        {
            if (CanExecute(parameter))
                ExecuteAction(parameter);
        }
    }


    public class RelayCommand<T> : RelayCommand
    {
        public RelayCommand(Func<T, bool> canExecute, Action<T> execute) : base(p => execute((T)p), p => canExecute((T)p))
        {
        }
    }
}