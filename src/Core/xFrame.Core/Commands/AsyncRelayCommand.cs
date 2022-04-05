using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using xFrame.Core.ExtensionMethodes;

namespace xFrame.Core.Commands
{
    public sealed class AsyncRelayCommand : IAsyncRelayCommand
    {
        private readonly Func<CancellationToken, Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly IErrorHandler _errorHandler;

        private bool _isRunning;
        private CancellationTokenSource _tokenSource;

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (_isRunning == value)
                    return;

                _isRunning = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
            }
        }

        public AsyncRelayCommand(Func<CancellationToken, Task> execute, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public void Cancel()
        {
            _tokenSource?.Cancel();
        }

        private bool CanExecute()
        {
            return !IsRunning && _canExecute?.Invoke() != false;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        public void Execute(object parameter)
        {
            _tokenSource = new CancellationTokenSource();
            ExecuteAsync(_tokenSource.Token).Await(_errorHandler);
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!CanExecute())
                return;

            try
            {
                IsRunning = true;
                NotifyCanExecuteChanged();
                await _execute(cancellationToken);
            }
            finally
            {
                IsRunning = false;
                NotifyCanExecuteChanged();
            }

        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }


    }

    public sealed class AsyncRelayCommand<T> : IAsyncRelayCommand<T>
    {
        private readonly Func<T, CancellationToken, Task> _execute;
        private readonly Predicate<T> _canExecute;
        private readonly IErrorHandler _errorHandler;

        private bool _isRunning;
        private CancellationTokenSource _tokenSource;
        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (_isRunning == value)
                    return;

                _isRunning = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
            }
        }

        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;


        public AsyncRelayCommand(Func<T,CancellationToken, Task> execute, Predicate<T> canExecute, IErrorHandler errorHandler)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(_execute)); 
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }

        public void Cancel()
        {
            _tokenSource?.Cancel();
        }

        public bool CanExecute(T parameter)
        {
            return !IsRunning && _canExecute?.Invoke(parameter) != false;
        }

        public bool CanExecute(object parameter)
        {
            return CanExecute((T)Convert.ChangeType(parameter, typeof(T)));
        }

        public void Execute(object parameter)
        {
            Execute((T)Convert.ChangeType(parameter, typeof(T)));
        }
        public void Execute(T parameter)
        {
            _tokenSource = new CancellationTokenSource();
            ExecuteAsync(parameter,_tokenSource.Token).Await(_errorHandler);
        }

        public async Task ExecuteAsync(T parameter, CancellationToken cancellationToken)
        {
            if (!CanExecute(parameter))
                return;

            try
            {
                IsRunning = true;
                await _execute(parameter, cancellationToken);
            }
            finally
            {
                IsRunning = false;
            }

            NotifyCanExecuteChanged();
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
