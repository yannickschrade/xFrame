using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using xFrame.Core.IoC;

namespace xFrame.Core.Modularity
{

    //TODO: Change adding loadingteps with fluent way
    // example : ModuleLoader.For<IUIModule>()
    //           .RegisterTypes()
    //           .Execute(() => ...)
    //           .InThread(xFrameApp.MainThread)
    public class ModuleManager : IModuleManager
    {

        private readonly List<Type> _modules = new List<Type>();
        private readonly List<IModule> _loadedModules = new List<IModule>();
        private readonly List<IModuleLoader> _moduleLoaders = new List<IModuleLoader>();

        public Dictionary<Type, Func<Type, IModule>> ModuleFactorys { get; }

        public IEnumerable<IModule> LoadedModules => _loadedModules;


        public ModuleManager()
        {
            ModuleFactorys = new Dictionary<Type, Func<Type, IModule>>();
        }

        public IModuleManager AddModule<T>()
        {
            AddModule(typeof(T));
            return this;
        }

        public IModuleManager AddModule(Type moduleType)
        {
            _modules.Add(moduleType);
            return this;
        }

        public IModuleManager AddModulesFromFolder(string path)
        {
            var files = Directory.GetFiles(path, "*.dll");
            foreach (var file in files)
            {
                var modules = Assembly.Load(file)
                    .GetTypes()
                    .Where(t => typeof(IModule).IsAssignableFrom(t));

                _modules.AddRange(modules);
            }
            return this;
        }

        public void LoadModules()
        {
            throw new NotImplementedException();
        }

        public IModuleManager AddModuleLoader<TModule>(IModuleLoader<TModule> moduleLoader) 
            where TModule : IModule
        {
            _moduleLoaders.Add(moduleLoader);
            return this;
        }

        public IModuleManager AddModuleLoader<TModule>(Action<IModuleLoaderBuilder<TModule>> action) where TModule : IModule
        {
            var builder = new ModuleLoaderBuilder<TModule>();
            action(builder);
            _moduleLoaders.Add(builder.ModuleLoader);
            return this;
        }
    }
}