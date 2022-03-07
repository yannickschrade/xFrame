using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace xFrame.WPF.Host
{
    public interface IXFrameAppSetup
    {
        void RunSetup(Application app);
    }
}
