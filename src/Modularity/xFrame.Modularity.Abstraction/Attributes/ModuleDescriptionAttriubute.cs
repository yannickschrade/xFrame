using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Modularity.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleDescriptionAttriubute : Attribute
    {
        public string ModuleName { get; set; }

        public ModuleDescriptionAttriubute(string moduleName)
        {
            ModuleName = moduleName;
        }
    }
}
