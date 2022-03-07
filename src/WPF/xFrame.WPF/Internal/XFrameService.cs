using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using xFrame.Core.Modularity;
using xFrame.WPF.Extensions;
using xFrame.WPF.Host;

namespace xFrame.WPF
{
    internal class XFrameHostedService : IHostedService
    {
        
        private XFrameContext _xFrameContext;
        private IServiceProvider _services;
        private ILogger<XFrameHostedService> _logger;
        
        public XFrameHostedService(ILogger<XFrameHostedService> logger, XFrameContext xFrameContext, IServiceProvider services)
        {
            _xFrameContext = xFrameContext;
            _logger = logger;
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested)
                return Task.CompletedTask;

            var thread = new Thread(XFrameThread);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_xFrameContext.IsRunning)
            {
                _xFrameContext.IsRunning = false;
                _logger.LogInformation("Shutdown xFrame app because the host stoped");
                await _xFrameContext.Dispatcher.InvokeAsync(() => _xFrameContext.Application.Shutdown());
            }
        }

        private void XFrameThread(object obj)
        {
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));

            var app = new XFrameApp(_services, _xFrameContext.ShellViewModelType);
            app.Exit += (s, e) => HandleExit();
            _xFrameContext.Dispatcher = Dispatcher.CurrentDispatcher;
            _logger.LogInformation("Starting xFrame app");
            _xFrameContext.IsRunning = true;
            _xFrameContext.Application = app;
            app.Run();
        }

        private void HandleExit()
        {
            if (!_xFrameContext.IsRunning)
                return;
            _xFrameContext.IsRunning = false;
            if (!_xFrameContext.IsLifetimeLinked)
            {
                return;
            }
            _logger.LogInformation("Stopping host because the xFrame app was closed");
            _services.GetService<IHostApplicationLifetime>()?.StopApplication();

        }
    }
}
