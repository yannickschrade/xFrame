using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.Modularity
{
    internal class ModuleLoaderBuilder<TModule> : IModuleLoaderBuilder<TModule>
        where TModule : IModule
    {
        public ModuleLoader<TModule> ModuleLoader { get; } = new ModuleLoader<TModule>();
        

        public IModuleLoaderBuilder<TModule> AddPhase(ILoadingPhase<TModule> phase)
        {
            ModuleLoader.AddPhase(phase);
            return this;
        }

        public IModuleLoaderBuilder<TModule> AddPhase(Action<ILoadingPhaseBuilder<TModule>> action)
        {
            var builder = new LoadingPhaseBuilder<TModule>();
            action(builder);
            ModuleLoader.AddPhase(builder.LoadingPhase);
            return this;
        }

        public IModuleLoaderBuilder<TModule> Factory(Func<Type, TModule> factory)
        {
            ModuleLoader.ModuleFactory = factory;
            return this;
        }

        public IModuleLoaderBuilder<TModule> Name(string name)
        {
            ModuleLoader.Name = name;
            return this;
        }
    }
}
