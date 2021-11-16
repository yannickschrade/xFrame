using System;

namespace xFrame.Core.Modularity
{
    public interface IModuleInfo
    {
        Type Type { get; }
        ModuleState State { get; set; }
        string Name { get; }
        Version Version { get; }
        int Priority { get; }
        ModuleType ModuleType { get; }
    }
}