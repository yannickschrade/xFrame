using System;
using System.Windows.Input;

namespace xFrame.Core.Commands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        protected bool _canExecute = true;
        protected readonly Action<object> ExecuteAction;
        protected readonly Func<object, bool> CanExecuteFunc;

        protected CommandBase(Action<object> executeAction, Func<object, bool> canExecuteFunc = null)
        {
            ExecuteAction = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
            CanExecuteFunc = canExecuteFunc;
        }

        public virtual bool CanExecute(object parameter)
        {
            if (CanExecuteFunc == null)
                return _canExecute;

            return CanExecuteFunc(parameter);
        }

        public abstract void Execute(object parameter);

        public void RaisCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        public void RaisCanExecuteChanged(bool canExecute)
        {
            _canExecute = canExecute;
            OnCanExecuteChanged();
        }

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}