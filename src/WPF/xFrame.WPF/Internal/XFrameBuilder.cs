using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using xFrame.WPF.Host;

namespace xFrame.WPF.Internal
{
    public class XFrameBuilder : IXFrameBuilder
    {
        public ShutdownMode ShutdownMode { get; set; }
        public Type ShellType { get; set; }
        public bool IsLifetimeLinked { get; set; }
    }
}
