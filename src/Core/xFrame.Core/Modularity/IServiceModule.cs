using Microsoft.Extensions.DependencyInjection;

namespace xFrame.Core.Modularity
{
    public interface IServiceModule : IModule
    {
        void RegisterServices(IServiceCollection services);
    }
}
