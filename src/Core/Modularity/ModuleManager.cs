using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace xFrame.Core.Modularity
{
    public class ModuleManager : IModuleManager
    {

        private Func<Type, IModule> _defaultModuleFactory;
        private readonly List<Type> _modules = new List<Type>();
        private readonly List<IModule> _loadedModules = new List<IModule>();

        
        public List<LoadingStep> LoadingSteps { get; }
        public Dictionary<Type, Func<Type, IModule>> ModuleFactorys { get; }

        public IEnumerable<IModule> LoadedModules => _loadedModules;


        public ModuleManager()
        {
            _defaultModuleFactory = CreateModule;
            ModuleFactorys = new Dictionary<Type, Func<Type, IModule>>();
            LoadingSteps = new List<LoadingStep>();
        }

        public void AddModule<T>()
        {
            AddModule(typeof(T));
        }

        public void AddModule(Type moduleType)
        {
            _modules.Add(moduleType);
        }

        public void AddModulesFromFolder(string path)
        {
            var files = Directory.GetFiles(path, "*.dll");
            foreach (var file in files)
            {
                var modules = Assembly.Load(file)
                    .GetTypes()
                    .Where(t => typeof(IModule).IsAssignableFrom(t));

                _modules.AddRange(modules);
            }
        }

        public void AddLoadingStep<T>(Action<T> action, LoadingType loadingType)
           where T : IModule
        {
            var step = new LoadingStep<T>(action, loadingType);
            LoadingSteps.Add(step);
        }

        public void AddLoadingStep<T>(LoadingStep<T> loadingStep)
            where T : IModule
        {
            LoadingSteps.Add(loadingStep);
        }


        public void AddModuleFactory<T>(Func<Type, IModule> moduleFactory)
        {
            ModuleFactorys[typeof(T)] = moduleFactory;
        }

        public void LoadModules()
        {
            var createdModules = new List<IModule>();

            foreach (var moduleType in _modules)
            {
                IModule module = null;
                if (ModuleFactorys.ContainsKey(moduleType))
                {
                    module = ModuleFactorys[moduleType](moduleType);
                }

                if (module == null)
                    module = _defaultModuleFactory(moduleType);

                createdModules.Add(module);
            }

            foreach (var loadingStep in LoadingSteps.OrderBy(s => s.LoadingType))
            {
                foreach (var module in createdModules)
                {
                    if (!loadingStep.ModuleType.IsAssignableFrom(module.GetType()))
                        continue;

                    loadingStep.Action(module);
                }
            }
        }


        private IModule CreateModule(Type moduleType)
        {
            return (IModule)Activator.CreateInstance(moduleType);
        }

       
    }
}