namespace xFrame.Core.IoC;

public interface ITypeProviderService
{
    object Resolve(Type type);
}
