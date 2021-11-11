namespace xFrame.Core.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ModulePriorityAttribute : Attribute
{
    public int Priority { get; }

    public ModulePriorityAttribute(int priority)
    {
        Priority = priority;
    }
}
