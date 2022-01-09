namespace xFrame.Core.IoC
{
    public interface ITypeService : ITypeRegistrationService, ITypeProviderService
    {

    }

    public interface ITypeService<T> : ITypeService
    {
        T TypeService { get; }
    }
}