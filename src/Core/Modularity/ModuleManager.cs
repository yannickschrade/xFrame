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
        public static Func<Type, IModule> DefaultModuleFactory { get; set; }

        private readonly List<Type> _moduleTypes = new List<Type>();
        private readonly List<IModule> _loadedModules = new List<IModule>();
        private readonly Dictionary<Type, IModuleLoader> _moduleLoaders = new Dictionary<Type, IModuleLoader>();
        private readonly Dictionary<IModule, IModuleLoader> _loaderForModule = new Dictionary<IModule, IModuleLoader>();

        public Dictionary<Type, Func<Type, IModule>> ModuleFactorys { get; }

        public IEnumerable<IModule> LoadedModules => _loadedModules;


        public ModuleManager(ITypeService typeService)
        {
            ModuleFactorys = new Dictionary<Type, Func<Type, IModule>>();
            if(DefaultModuleFactory == null)
                DefaultModuleFactory = p => (IModule)TypeService.Current.Resolve(p);
        }

        public IModuleManager AddModule<T>()
        {
            AddModule(typeof(T));
            return this;
        }

        public IModuleManager AddModule(Type moduleType)
        {
            _moduleTypes.Add(moduleType);
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

                _moduleTypes.AddRange(modules);
            }
            return this;
        }

        public void LoadModules()
        {
            foreach (var moduleType in _moduleTypes)
            {
                var loader = GetLoaderForModule(moduleType);
                var module = loader.CreateModule(moduleType);
                _loaderForModule.Add(module, loader);
            }

            foreach (var module in _loaderForModule.Keys)
            {
                var loader = _loaderForModule[module];
                loader.LoadModule(module);
            }
        }

        public IModuleManager AddModuleLoader<TModule>(IModuleLoader<TModule> moduleLoader)
            where TModule : IModule
        {
            _moduleLoaders.Add(moduleLoader.ForType, moduleLoader);
            return this;
        }

        public IModuleManager AddModuleLoader<TModule>(Action<IModuleLoaderBuilder<TModule>> action) where TModule : IModule
        {
            var builder = new ModuleLoaderBuilder<TModule>();
            action(builder);
            _moduleLoaders.Add(builder.ModuleLoader.ForType, builder.ModuleLoader);
            return this;
        }

        private IModuleLoader GetLoaderForModule(Type moduleType)
        {
            var basetype = moduleType;


            var interfaces = basetype.GetInterfaces();
            foreach (var interfaceType in interfaces)
            {
                if (_moduleLoaders.TryGetValue(interfaceType, out var loader))
                    return loader;
            }

            throw new KeyNotFoundException($" No moduleloader found for {moduleType}");
        }
    }
}