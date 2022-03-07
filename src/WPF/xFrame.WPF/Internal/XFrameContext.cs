using System;
using System.Windows;
using System.Windows.Threading;
using xFrame.WPF.Host;

namespace xFrame.WPF.Extensions
{
    internal class XFrameContext : IXFrameContext
    {
        public ShutdownMode ShutdownMode { get; set; }
        public Dispatcher Dispatcher { get; set; }
        public bool IsLifetimeLinked { get; set; }
        public bool IsRunning { get; set; }
        public Type ShellViewModelType { get; set; }
        public Application Application { get; set; }
    }
}