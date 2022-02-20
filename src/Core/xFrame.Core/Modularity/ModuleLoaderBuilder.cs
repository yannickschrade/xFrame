using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    internal class ModuleLoaderBuilder<TModule> : IModuleLoaderBuilder<TModule>
        where TModule : IModule
    {
        public ModuleLoader<TModule> ModuleLoader { get; private set; }
        public string Name
        {

            get => ModuleLoader.Name;
            set => ModuleLoader.Name = value;
        }
        public Func<Type, TModule> ModuleFactory
        {
            get => ModuleLoader.ModuleFactory;
            set => ModuleLoader.ModuleFactory = value;
        }

        public ModuleLoaderBuilder()
        {
            ModuleLoader = new ModuleLoader<TModule>();
        }

        public IModuleLoaderBuilder<TModule> AddPhase(ILoadingPhase<TModule> phase)
        {
            ModuleLoader.AddPhase(phase);
            return this;
        }

        public IModuleLoaderBuilder<TModule> AddPhase(object key, Action<ILoadingPhaseBuilder<TModule>> action)
        {
            var phase = new LoadingPhase<TModule>(key);
            var builder = new LoadingPhaseBuilder<TModule>(phase);
            action(builder);
            ModuleLoader.AddPhase(builder.LoadingPhase);
            return this;
        }
    }
}
