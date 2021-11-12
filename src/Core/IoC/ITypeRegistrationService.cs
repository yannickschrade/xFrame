namespace xFrame.Core.IoC;

public interface ITypeRegistrationService
{
    ITypeRegistrationService RegisterType(Type from, Type to);

    ITypeRegistrationService RegisterType(Type to, Func<object> factory);

    ITypeRegistrationService RegisterType(Type to, Func<ITypeRegistrationService, object> factory);

    ITypeRegistrationService RegisterInstance(Type to, object instance);

    ITypeRegistrationService RegisterSingelton(Type from, Type to);

    ITypeRegistrationService RegisterSingelton(Type to, Func<object> factory);

    ITypeRegistrationService RegisterSingelton(Type to, Func<ITypeRegistrationService, object> factory);
}
