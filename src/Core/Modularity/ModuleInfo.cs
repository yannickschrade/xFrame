using System;
using System.Reflection;

namespace xFrame.Core.Modularity
{
    public class ModuleInfo : IModuleInfo
    {
        public Assembly ModuleAssembly { get; }
        public Type Type { get; }
        public ModuleState State { get; set; }
        public string Name { get; }
        public Version Version { get; }
        public int Priority { get; }
        public ModuleType ModuleType { get; }
        public IModule Instance { get; set; }

        public ModuleInfo(Assembly assembly, Type type, string name, Version version = null, int priority = 0, ModuleType moduleType = ModuleType.Undefined)
        {
            ModuleAssembly = assembly;
            Type = type;
            Name = name;
            Version = version ?? new Version();
            Priority = priority;
            ModuleType = moduleType;
        }
    }
}