using xFrame.Core.IoC;

namespace xFrame.Core.Modularity.DefaultPhases
{
    public class InitialisationPhase<TModule> : LoadingPhase<TModule>
        where TModule: IModule
    {
        public InitialisationPhase()
            :base(DefaultLoadingPhase.ModuleInitialisation)
        {
            Name = "DefaultInitialisation";
            AddLoadingAction(m => m.InitializeModule(TypeService.Current));
        }
    }
}
