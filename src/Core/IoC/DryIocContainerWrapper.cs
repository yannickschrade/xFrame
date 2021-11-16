using DryIoc;
using xFrame.Core.IoC;

namespace xFrame.Core.IoC;

public class DryIocContainerWrapper : ITypeService<IContainer>
{

    public static Rules DefaultRules => Rules.Default.WithConcreteTypeDynamicRegistrations(reuse: Reuse.Transient)
                                                        .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
                                                        .WithFuncAndLazyWithoutRegistration()
                                                        .WithTrackingDisposableTransients()
                                                        .WithoutFastExpressionCompiler()
                                                        .WithFactorySelector(Rules.SelectLastRegisteredFactory());
    public IContainer TypeService { get; private set; }

    public DryIocContainerWrapper()
    {
        TypeService = new Container(DefaultRules);
        TypeService.RegisterInstanceMany(new[]
        {
            typeof(ITypeProviderService),
            typeof(ITypeRegistrationService)
        }, this);
        TypeService.RegisterInstance(TypeService);
    }

    public DryIocContainerWrapper(IContainer container)
    {
        TypeService = container;
        TypeService.RegisterInstanceMany(new[]
        {
            typeof(IContainer),
            typeof(ITypeService),
            typeof(ITypeProviderService),
            typeof(ITypeRegistrationService)
        }, this);
    }

    public void FinalizeTypeService() { }

    public ITypeRegistrationService RegisterInstance(Type to, object instance)
    {
        TypeService.RegisterInstance(to, instance);
        return this;
    }

    public ITypeRegistrationService RegisterSingelton(Type from, Type to)
    {
        TypeService.Register(to, from, Reuse.Singleton);
        return this;
    }

    public ITypeRegistrationService RegisterSingelton(Type to, Func<object> factory)
    {
        TypeService.RegisterDelegate(to, c => factory(), Reuse.Singleton);
        return this;
    }

    public ITypeRegistrationService RegisterSingelton(Type to, Func<ITypeRegistrationService, object> factory)
    {
        TypeService.RegisterDelegate(to, c => factory(this), Reuse.Singleton);
        return this;
    }

    public ITypeRegistrationService RegisterType(Type from, Type to)
    {
        TypeService.Register(to, from);
        return this;
    }

    public ITypeRegistrationService RegisterType(Type to, Func<object> factory)
    {
        TypeService.RegisterDelegate(to, c => factory());
        return this;
    }

    public ITypeRegistrationService RegisterType(Type to, Func<ITypeRegistrationService, object> factory)
    {
        TypeService.RegisterDelegate(to, c => factory(this));
        return this;
    }

    public ITypeRegistrationService RegisterType(Type from, Type to, string name)
    {
        TypeService.Register(to, from, serviceKey: name);
        return this;
    }

    public ITypeRegistrationService RegisterMany(Type type, params Type[] services)
    {
        TypeService.RegisterMany(services, type);
        return this;
    }

    public ITypeRegistrationService RegisterInstanceMany(object instance, params Type[] services)
    {
        TypeService.RegisterInstanceMany(services, instance);
        return this;
    }

    public ITypeRegistrationService RegisterSingeltonMany(Type type, params Type[] services)
    {
        TypeService.RegisterMany(services, type, reuse: Reuse.Singleton);
        return this;
    }

    public object Resolve(Type type)
    {
        return TypeService.Resolve(type);
    }

    public object Resolve(Type type, params (Type type, object Instance)[] parameters)
    {
        return TypeService.Resolve(type, args: parameters.Select(p => p.Instance).ToArray());
    }

    public object Resolve(Type type, string name)
    {
        return TypeService.Resolve(type, name);
    }

    public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
    {
        return TypeService.Resolve(type, name, args: parameters.Select(p => p.Instance).ToArray());
    }

    public bool IsRegistered(Type type, string? name = null)
    {
        return TypeService.IsRegistered(type, name);
    }
}
public static class DryIoCExtensionMethodes
{
    public static ITypeService Wrap(this IContainer container)
    {
        return new DryIocContainerWrapper(container);
    }
}