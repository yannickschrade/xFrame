using xFrame.Core.IoC;

namespace xFrame.Core.Modularity.DefaultPhases
{
    public class InitialisationPhase<TModule> : LoadingPhase<TModule>
        where TModule: IModule
    {
        public InitialisationPhase(ITypeProviderService typeProvider)
            :base(DefaultLoadingPhase.ModuleInitialisation)
        {
            Name = "DefaultInitialisation";
            AddAction(x =>
                x.AddExecute(m => m.InitializeModule(typeProvider))
            );
        }
    }
}
