namespace CSU.Core.IoC;

public interface IContainer : IRegistrationContainer, IResolverContainer
{

}

public interface IContainer<T> : IContainer
{
    T Instance { get; }
}
