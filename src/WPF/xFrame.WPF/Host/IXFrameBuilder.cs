using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace xFrame.WPF.Host
{
    public interface IXFrameBuilder
    {
        ShutdownMode ShutdownMode { get; set; }
        Type ShellType { get; set; }
        bool IsLifetimeLinked { get; set; }
    }
}
