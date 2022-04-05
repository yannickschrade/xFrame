using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace xFrame.Core.Commands
{
    public interface IAsyncRelayCommand : IRelayCommand, INotifyPropertyChanged
    {
        bool IsRunning { get; }
        
        Task ExecuteAsync(CancellationToken cancellationToken);
        void Cancel();

    }

    public interface IAsyncRelayCommand<T> : IRelayCommand<T>, INotifyPropertyChanged
    {

        bool IsRunning { get; }

        Task ExecuteAsync(T parameter, CancellationToken cancellationToken);
        void Cancel();
    }
}
