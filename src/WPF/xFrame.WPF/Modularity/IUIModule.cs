using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.Modularity;
using xFrame.Core.ViewInjection;

namespace xFrame.WPF.Modularity
{
    public interface IUIModule : IServiceModule
    {
        void SetupUI(IViewInjectionService viewInjectionService);
    }
}
