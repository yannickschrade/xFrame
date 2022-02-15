using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xFrame.Core.IoC;

namespace xFrame.Core.Modularity.DefaultPhases
{
    public class RegistrationPhase<TModule> : LoadingPhase<TModule>
        where TModule : IModule
    {
        public RegistrationPhase()
            : base(DefaultLoadingPhase.TypeRegistration)
        {
            AddAction(x => x.AddExecute(m => m.RegisterServices(TypeService.Current)));
        }
    }
}
