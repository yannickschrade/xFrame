using System;

namespace xFrame.Core.Modularity
{
    public class ModuleInfo : IModuleInfo
    {
        public Type Type { get; }
        public ModuleState State { get; set; }
        public string Name { get; }
        public Version Version { get; }
        public int Priority { get; }
        public ModuleType ModuleType { get; }

        public ModuleInfo(Type type, string name, Version version = null, int priority = 0, ModuleType moduleType = ModuleType.Undefined)
        {
            Type = type;
            Name = name;
            Version = version ?? new Version();
            Priority = priority;
            ModuleType = moduleType;
        }
    }
}