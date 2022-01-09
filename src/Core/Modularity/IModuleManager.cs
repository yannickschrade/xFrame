using System;
using System.Collections.Generic;

namespace xFrame.Core.Modularity
{
    public interface IModuleManager
    {
        IEnumerable<IModule> LoadedModules { get; }

        public Dictionary<Type, Func<Type, IModule>> ModuleFactorys { get; }
        void AddModule<T>();
        void AddModule(Type moduleType);
        void AddModulesFromFolder(string path);

        void AddModuleFactory<T>(Func<Type, IModule> moduleFactory);

        void AddLoadingStep<T>(Action<T> step, LoadingType loadingType)
            where T : IModule;

        void LoadModules();
    }
}