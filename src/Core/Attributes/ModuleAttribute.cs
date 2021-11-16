using System;
using xFrame.Core.Modularity;

namespace xFrame.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ModuleAttribute : Attribute
    {
        public string ModuleName { get; }
        public Version Version { get; }
        public int Priority { get; }
        public ModuleType ModuleType { get; }

        public ModuleAttribute(string name, Version version = null, int priority = 0, ModuleType moduleType = ModuleType.Undefined)
        {
            ModuleName = name;
            Version = version ?? new Version();
            Priority = priority;
            ModuleType = moduleType;
        }
    }
}