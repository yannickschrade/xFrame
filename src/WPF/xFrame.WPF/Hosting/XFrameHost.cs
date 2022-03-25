using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace xFrame.WPF.Hosting
{
    public sealed class XFrameHost : IHost, IAsyncDisposable
    {
        private readonly IHost _host;
        public IServiceProvider Services => _host.Services;

        internal XFrameHost(IHost host)
        {
            _host = host;
        }

        public static XFrameHostBuilder CreateBuilder(bool useDefaults = true)
        {
            return new XFrameHostBuilder(useDefaults);
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
            => _host.StartAsync(cancellationToken);


        public Task StopAsync(CancellationToken cancellationToken = default)
            => _host.StopAsync(cancellationToken);


        public void Dispose()
            => _host.Dispose();


        public ValueTask DisposeAsync()
            => ((IAsyncDisposable)_host).DisposeAsync();
    }
}
