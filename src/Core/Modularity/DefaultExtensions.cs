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
    }
}
