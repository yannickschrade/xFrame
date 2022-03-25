using System.Windows.Threading;
using System.Windows;

namespace xFrame.WPF.Hosting
{
    public sealed class XFrameAppContext
    {
        public ShutdownMode ShutdownMode { get; set; }
        public Dispatcher Dispatcher { get; internal set; }
        public bool IsLifetimeLinked { get; set; } = true;
        public bool IsRunning { get; internal set; }

        internal XFrameAppContext()
        {

        }
    }
}
