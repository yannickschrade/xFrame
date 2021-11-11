namespace xFrame.Core.IoC;

public interface IResolverContainer
{
    object Resolve(Type type);
}
