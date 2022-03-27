using Microsoft.Extensions.DependencyInjection;

namespace xFrame.Core.Modularity
{
    public interface IServiceModule
    {
        void RegisterServices(IServiceCollection services);
    }
}
