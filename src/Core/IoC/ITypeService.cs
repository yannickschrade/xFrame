namespace xFrame.Core.IoC;

public interface ITypeService : ITypeRegistrationService, ITypeResolverService
{

}

public interface ITypeService<T> : ITypeService
{
    T TypeService { get; }
}
