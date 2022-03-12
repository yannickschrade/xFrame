using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.ViewInjection;
using xFrame.Modularity.Abstraction;

namespace xFrame.WPF.Modularity
{
    public interface IUIModule : IModule
    {
        void SetupUI(IViewInjectionService viewInjectionService);
    }
}
