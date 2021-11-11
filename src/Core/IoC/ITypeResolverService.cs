namespace xFrame.Core.IoC;

public interface ITypeResolverService
{
    object Resolve(Type type);
}
