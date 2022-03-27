using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xFrame.Core.Modularity;

namespace xFrame.WPF.Hosting
{
    internal class ModuleProvider : IModuleProvider
    {
        private readonly IModuleCollection _modules;
        private readonly IModuleLoaderCollection _moduleLoaders;
        private readonly IServiceProvider _services;
        private readonly ILogger<ModuleProvider> _logger;


        public ModuleProvider(IModuleCollection modules, IModuleLoaderCollection moduleLoaders, IServiceProvider serviceProvider)
        {
            _modules = modules;
            _moduleLoaders = moduleLoaders;
            _services = serviceProvider;
            _logger = serviceProvider.GetService<ILogger<ModuleProvider>>();
        }

        public void LoadAllModules(Action<IModule> loadedCallback)
        {
            var modules = _modules.Where(m => m.ModuleType == ModuleType.FunctionalModule);
            foreach (var moduleDescription in modules)
            {
                var loader = _moduleLoaders.GetLoaderFor(moduleDescription.ImplementationType);
                if (loader == null)
                {
                    _logger.LogInformation("No loader for {moduleType} found", moduleDescription.ImplementationType);
                    continue;
                }
                var module = loader.CreateModule(_services, moduleDescription.ImplementationType);
                loader.InitializeModule(_services, module);
                loadedCallback(module);
            }
        }
    }
}
