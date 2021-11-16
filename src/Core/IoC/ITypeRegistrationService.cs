namespace xFrame.Core.IoC;

public interface ITypeRegistrationService
{
    ITypeRegistrationService RegisterType(Type from, Type to);

    ITypeRegistrationService RegisterType(Type from, Type to, string name);

    ITypeRegistrationService RegisterType(Type to, Func<object> factory);

    ITypeRegistrationService RegisterType(Type to, Func<ITypeRegistrationService, object> factory);

    ITypeRegistrationService RegisterMany(Type type, params Type[] services);

    ITypeRegistrationService RegisterInstance(Type to, object instance);

    ITypeRegistrationService RegisterInstanceMany(object instance, params Type[] services);

    ITypeRegistrationService RegisterSingelton(Type from, Type to);

    ITypeRegistrationService RegisterSingelton(Type to, Func<object> factory);

    ITypeRegistrationService RegisterSingeltonMany(Type type, params Type[] services);

    ITypeRegistrationService RegisterSingelton(Type to, Func<ITypeRegistrationService, object> factory);

    bool IsRegistered(Type type, string? name = null);
}
