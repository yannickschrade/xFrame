using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace xFrame.WPF.Hosting.Internal
{
    internal class HostedAppService : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly XFrameAppContext _appContext;
        private readonly ILogger<HostedAppService> _logger;
        private readonly Application app;

        public HostedAppService(IServiceProvider services, XFrameAppContext appContext, ILogger<HostedAppService> logger)
        {
            _services = services;
            _appContext = appContext;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return Task.CompletedTask;

            var thread = new Thread(XFrameThread);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_appContext.IsRunning)
            {
                _appContext.IsRunning = false;
                _logger.LogInformation("Shutdown app because the host was stoped");
                await _appContext.Dispatcher.InvokeAsync(() => app.Shutdown());
            }
        }

        private void XFrameThread(object obj)
        {
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));

            var app = _services.GetRequiredService<Application>();
            app.Exit += (s, e) => HandleExit();
            _appContext.Dispatcher = Dispatcher.CurrentDispatcher;

            _logger.LogInformation("Starting app");

            foreach (var action in _services.GetServices<Action<Application>>())
            {
                action(app);
            }

            foreach (var action in _services.GetServices<Action<ResourceDictionary>>())
            {
                action(app.Resources);
            }

            _appContext.IsRunning = true;
            app.Run();
        }

        private void HandleExit()
        {
            if (!_appContext.IsRunning)
                return;
            _appContext.IsRunning = false;
            if (!_appContext.IsLifetimeLinked)
            {
                return;
            }
            _logger.LogInformation("Stopping host because the app was closed");
            _services.GetService<IHostApplicationLifetime>()?.StopApplication();

        }
    }
}
