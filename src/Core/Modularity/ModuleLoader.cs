using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace xFrame.Core.Modularity
{
    public class ModuleLoader<TModule> : IModuleLoader<TModule>
        where TModule : IModule
    {
        private readonly List<ILoadingPhase<TModule>> _phases = new List<ILoadingPhase<TModule>>();
        public Type ForType => typeof(TModule);
        public string Name { get; set; }
        public Func<Type, TModule> ModuleFactory { get; set; }
        Func<Type, object> IModuleLoader.ModuleFactory => p => ModuleFactory;

        public IEnumerable<ILoadingPhase<TModule>> LoadingPhases => _phases;
        IEnumerable<ILoadingPhase<IModule>> IModuleLoader.LoadingPhases => (IEnumerable<ILoadingPhase<IModule>>)LoadingPhases;


        public ModuleLoader()
        {
            Name = ToString();
            ModuleFactory = p =>  (TModule)ModuleManager.DefaultModuleFactory(p);
        }

        public IModuleLoader<TModule> AddPhase(object key,Action<ILoadingPhaseBuilder<TModule>> action)
        {
            var phase = new LoadingPhase<TModule>(key);
            var builder = new LoadingPhaseBuilder<TModule>(phase);
            action(builder);
            _phases.Add(builder.LoadingPhase);
            return this;
        }

        public IModuleLoader<TModule> AddPhase(ILoadingPhase<TModule> loadingPhase)
        {
            _phases.Add(loadingPhase);
            return this;
        }

        public TModule CreateModule(Type moduleType)
        {
            var module = ModuleFactory(moduleType);
            var registerPhase = _phases.FirstOrDefault(p => DefaultLoadingPhase.TypeRegistration.Equals(p.Key));
            registerPhase?.Run(module);
            _phases.Remove(registerPhase);
            return module;
        }

        IModule IModuleLoader.CreateModule(Type moduleType)
        {
            return CreateModule(moduleType);
        }

        public void LoadModule(TModule module)
        {
            foreach (var phase in LoadingPhases)
            {
                phase.Run(module);
            }
        }

        public void LoadModule(IModule module)
        {
            LoadModule((TModule)module);
        }

        public IModuleLoader<TModule> EditPhase(object key, Action<ILoadingPhase<TModule>> phase)
        {
            var editedphase = _phases.First(p => p.Key.Equals(key));
            phase(editedphase);
            return this;
        }
    }
}
