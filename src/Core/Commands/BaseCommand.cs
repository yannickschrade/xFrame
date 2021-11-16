using System;
using System.Windows.Input;

namespace xFrame.Core.Commands
{
    public abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        protected readonly Action<object> ExecuteAction;
        protected readonly Func<object, bool> CanExecuteFunc;

        protected BaseCommand(Action<object> executeAction, Func<object, bool> canExecuteFunc = null)
        {
            ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            CanExecuteFunc = canExecuteFunc;
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        public void RaisCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}