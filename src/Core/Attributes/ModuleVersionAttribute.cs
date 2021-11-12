using System;

namespace xFrame.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ModuleVersionAttribute : Attribute
{
    public Version ModuleVersion { get; }

    public ModuleVersionAttribute(Version moduleVersion)
    {
        ModuleVersion = moduleVersion;
    }
}
