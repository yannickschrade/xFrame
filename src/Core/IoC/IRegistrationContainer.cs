namespace xFrame.Core.IoC;

public interface IRegistrationContainer
{
    IRegistrationContainer RegisterType(Type from, Type to);

    IRegistrationContainer RegisterType(Type to, Func<object> factory);

    IRegistrationContainer RegisterType(Type to, Func<IRegistrationContainer, object> factory);

    IRegistrationContainer RegisterInstance(Type to, object instance);

    IRegistrationContainer RegisterSingelton(Type from, Type to);

    IRegistrationContainer RegisterSingelton(Type to, Func<object> factory);

    IRegistrationContainer RegisterSingelton(Type to, Func<IRegistrationContainer, object> factory);


}
