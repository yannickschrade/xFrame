using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    public class ModuleLoader<TModule> : IModuleLoader<TModule>
        where TModule : IModule
    {
        private readonly List<ILoadingPhase<TModule>> _phases;
        public Type ForType => typeof(TModule);
        public string Name { get; set; }
        public Func<Type, TModule> ModuleFactory { get; set; }
        Func<Type, object> IModuleLoader.ModuleFactory => p => ModuleFactory(p);

        public IEnumerable<ILoadingPhase<TModule>> LoadingPhases => _phases;
        IEnumerable<ILoadingPhase<IModule>> IModuleLoader.LoadingPhases => (IEnumerable<ILoadingPhase<IModule>>)LoadingPhases;

        public ModuleLoader()
        {
            Name = ToString();
        }

        public IModuleLoader<TModule> AddPhase(Action<ILoadingPhaseBuilder<TModule>> action)
        {
            var builder = new LoadingPhaseBuilder<TModule>();
            action(builder);
            _phases.Add(builder.LoadingPhase);
            return this;
        }

        public IModuleLoader<TModule> AddPhase(ILoadingPhase<TModule> loadingPhase)
        {
            _phases.Add(loadingPhase);
            return this;
        }
    }
}
