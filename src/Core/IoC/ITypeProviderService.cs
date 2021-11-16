namespace xFrame.Core.IoC;

public interface ITypeProviderService
{
    object Resolve(Type type);

    object Resolve(Type type, params (Type type, object Instance)[] parameters);

    object Resolve(Type type, string name);

    object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters);

    bool IsRegistered(Type type, string? name = null);
}
