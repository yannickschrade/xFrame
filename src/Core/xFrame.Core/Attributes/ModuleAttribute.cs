using System;
using xFrame.Core.Modularity;

namespace xFrame.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModuleAttribute : Attribute
    {
        public string ModuleName { get; }
        public Version Version { get; }

        public ModuleAttribute(string name, Version version = null)
        {
            ModuleName = name;
            Version = version;
        }
    }
}