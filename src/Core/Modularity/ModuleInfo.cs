namespace xFrame.Core.Modularity;

public record class ModuleInfo : IModuleInfo
{
    public Type Type { get; init; }
    public ModuleState State { get; set; }
    public string Name { get; init; }
    public Version Version { get; init; }
    public int Priority { get; init; }
    public ModuleType ModuleType { get; init; }

    public ModuleInfo(Type type, string name, Version? version = null, int priority = 0, ModuleType moduleType = ModuleType.Undefined)
    {
        Type = type;
        Name = name;
        Version = version ?? new Version();
        Priority = priority;
        ModuleType = moduleType;
    }
}
