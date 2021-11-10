using CSU.Core.IoC;
using DryIoc;

namespace CSU.Core.IoC;

internal class DryIocContainerWrapper : IContainer<Container>
{
    public Container Instance { get; private set; }

    public DryIocContainerWrapper()
    {
        Instance = new Container();
    }

    public void FinalizeContainer() { }

    public IRegistrationContainer RegisterInstance(Type to, object instance)
    {
        Instance.RegisterInstance(to, instance);
        return this;
    }

    public IRegistrationContainer RegisterSingelton(Type from, Type to)
    {
        Instance.Register(from, to, Reuse.Singleton);
        return this;
    }

    public IRegistrationContainer RegisterSingelton(Type to, Func<object> factory)
    {
        Instance.RegisterDelegate(to, c => factory(), Reuse.Singleton);
        return this;
    }

    public IRegistrationContainer RegisterSingelton(Type to, Func<IRegistrationContainer, object> factory)
    {
        Instance.RegisterDelegate(to, c => factory(this), Reuse.Singleton);
        return this;
    }

    public IRegistrationContainer RegisterType(Type from, Type to)
    {
        Instance.Register(from, to);
        return this;
    }

    public IRegistrationContainer RegisterType(Type to, Func<object> factory)
    {
        Instance.RegisterDelegate(to, c => factory());
        return this;
    }

    public IRegistrationContainer RegisterType(Type to, Func<IRegistrationContainer, object> factory)
    {
        Instance.RegisterDelegate(to, c => factory(this));
        return this;
    }

    public object Resolve(Type type)
    {
        return Instance.Resolve(type);
    }
}
