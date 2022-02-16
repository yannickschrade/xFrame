using System;
using System.Collections.Generic;

namespace xFrame.Core.Modularity
{
    public interface IModuleManager
    {
        IEnumerable<IModule> LoadedModules { get; }

        public Dictionary<Type, Func<Type, IModule>> ModuleFactorys { get; }
        IModuleManager AddModule<T>();
        IModuleManager AddModule(Type moduleType);
        IModuleManager AddModulesFromFolder(string path);
        void LoadModules();

        void AddModuleLoader<TModule>(IModuleLoader<TModule> moduleLoader) where TModule : IModule;

        IModuleLoader<TModule> AddModuleLoader<TModule>(Action<IModuleLoaderBuilder<TModule>> builder)
            where TModule : IModule;

        void EditModuleLoader<TModule>(Action<IModuleLoader<TModule>> loader)
            where TModule : IModule;
    }
}