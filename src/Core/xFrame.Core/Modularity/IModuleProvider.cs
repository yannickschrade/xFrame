using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    public interface IModuleProvider
    {
        void LoadAllModules(Action<IModule> loadedCallback);
    }
}
