namespace CSU.Core.IoC;

public interface IResolverContainer
{
    object Resolve(Type type);
}
