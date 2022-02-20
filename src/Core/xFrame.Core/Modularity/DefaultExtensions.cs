using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.Modularity.DefaultPhases;

namespace xFrame.Core.Modularity
{
    public static class DefaultExtensions
    {
        public static IModuleLoaderBuilder<TModule> AddRegistrationPhase<TModule>(this IModuleLoaderBuilder<TModule> builder)
            where TModule : IModule
        {
            builder.AddPhase(new RegistrationPhase<TModule>());
            return builder;
        }

        public static IModuleLoaderBuilder<TModule> AddInitialisationPhase<TModule>(this IModuleLoaderBuilder<TModule> builder)
            where TModule: IModule
        {
            builder.AddPhase(new InitialisationPhase<TModule>());
            return builder;
        }

        public static void AddRegistrationPhase<TModule>(this IModuleLoader<TModule> loader)
            where TModule : IModule
        {
            loader.AddPhase(new RegistrationPhase<TModule>());
        }

        public static void AddInitialisationPhase<TModule>(this IModuleLoader<TModule> loader)
            where TModule : IModule
        {
            loader.AddPhase(new InitialisationPhase<TModule>());
        }
    }
}
