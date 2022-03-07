using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace xFrame.Core.Modularity
{

    //TODO: Change adding loadingteps with fluent way
    // example : ModuleLoader.For<IUIModule>()
    //           .RegisterTypes()
    //           .Execute(() => ...)
    //           .InThread(xFrameApp.MainThread)
    public class ModuleManager : IModuleManager
    {
       
    }
}