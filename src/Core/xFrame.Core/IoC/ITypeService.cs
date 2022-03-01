using System;

namespace xFrame.Core.IoC
{
    public interface ITypeService : ITypeRegistrationService, ITypeProviderService, IDisposable
    {

    }

    public interface ITypeService<T> : ITypeService
    {
        T TypeService { get; }
    }
}