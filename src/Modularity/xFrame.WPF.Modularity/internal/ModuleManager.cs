using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using xFrame.Modularity.Abstraction;
using xFrame.WPF.Modularity.@internal;

namespace xFrame.WPF.Modularity.Internal
{
    internal class ModuleManager : IModuleManager
    {
        private IModuleLoaderCollection _moduleLoaders;
        private IServiceProvider _services;
        private ILogger<ModuleManager> _logger;

        public event EventHandler<IModule> ModuleInitialisationStarted;
        public event EventHandler<IModule> ModuleInitialized;

        public List<IModule> RegisteredModules { get; }

        public ModuleManager(IModuleLoaderCollection moduleLoaders, IServiceProvider services,
            ILogger<ModuleManager> logger)
        {
            RegisteredModules = services.GetServices<IModule>().ToList();
            _moduleLoaders = moduleLoaders;
            _services = services;
            _logger = logger;
        }

        public void InitializeModules()
        {
            foreach (var module in RegisteredModules)
            {
                OnStartModuleInitialisation(module);
                var loader = _moduleLoaders.GetLoaderFor(module);
                if (loader == null)
                {
                    _logger.LogError("No module loader found for {Module}", module);
                    continue;
                }

                try
                {
                    loader.InitializeModule(_services, module);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "An error accord while loading {Module}", module);
                    continue;
                }

                OnModuleInitialized(module);
            }
        }

        private void OnStartModuleInitialisation(IModule module)
        {
            ModuleInitialisationStarted?.Invoke(this, module);
        }

        private void OnModuleInitialized(IModule module)
        {
            ModuleInitialized?.Invoke(this, module);
        }
    }
}