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

        IModuleManager AddModuleLoader<TModule>(IModuleLoader<TModule> moduleLoader) where TModule : IModule;

        IModuleManager AddModuleLoader<TModule>(Action<IModuleLoaderBuilder<TModule>> builder)
            where TModule : IModule;
    }
}