using System;
using System.Windows.Input;

namespace xFrame.Core.Commands
{
    public class RelayCommand : IRelayCommand
    {

        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() != false;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }


    public class RelayCommand<T> : IRelayCommand<T>
    {

        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(T parameter)
        {
            return _canExecute?.Invoke(parameter) != false;
        }

        public bool CanExecute(object parameter)
        {
            if(default(T) is not null && parameter is null)
            {
                return false;
            }

            return CanExecute((T)Convert.ChangeType(parameter, typeof(T)));
        }

        public void Execute(T parameter)
        {
            _execute(parameter);
        }

        public void Execute(object parameter)
        {
            Execute((T)Convert.ChangeType(parameter,typeof(T)));
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}