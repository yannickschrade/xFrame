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
    }

    public DryIocContainerWrapper(IContainer container)
    {
        TypeService = container;
    }

    public void FinalizeTypeService() { }

    public ITypeRegistrationService RegisterInstance(Type to, object instance)
    {
        TypeService.RegisterInstance(to, instance);
        return this;
    }

    public ITypeRegistrationService RegisterSingelton(Type from, Type to)
    {
        TypeService.Register(from, to, Reuse.Singleton);
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
        TypeService.Register(from, to);
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

    public object Resolve(Type type)
    {
        return TypeService.Resolve(type);
    }
}
public static class DryIoCExtensionMethodes
{
    public static ITypeService Wrap(this IContainer container)
    {
        return new DryIocContainerWrapper(container);
    }
}