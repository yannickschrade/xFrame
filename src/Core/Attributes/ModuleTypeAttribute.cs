using xFrame.Core.Modularity;

namespace xFrame.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ModuleTypeAttribute : Attribute
{
    public ModuleType ModuleType { get; }

    public ModuleTypeAttribute(ModuleType moduleType)
    {
        ModuleType = moduleType;
    }
}
