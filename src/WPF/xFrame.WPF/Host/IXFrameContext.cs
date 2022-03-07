using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace xFrame.WPF.Host
{
    public interface IXFrameContext
    {
        ShutdownMode ShutdownMode { get; }
        Type ShellViewModelType { get; }
        Dispatcher Dispatcher { get; }
        Application Application { get; }
        bool IsLifetimeLinked { get; }
        bool IsRunning { get; }
    }
}
