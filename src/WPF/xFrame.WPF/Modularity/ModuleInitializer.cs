using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using xFrame.Core.IoC;
using xFrame.Core.Modularity;
using xFrame.WPF.ViewInjection;

namespace xFrame.WPF.Modularity
{
    public sealed class ModuleInitializer : DefaultModuleInitializer
    {

        public ModuleInitializer(ITypeService typeService) : base(typeService)
        {
            Steps = new List<Action<IModuleInfo>>
            {
                RegisterTypes,
                RegisterViews,
                InitializeModule
            };
        }


        private void RegisterViews(IModuleInfo moduleInfo)
        {
            if (!(moduleInfo is IUiModule))
                return;
            var moduleAssembly = Assembly.GetAssembly(moduleInfo.Type);
            var viewTypes = moduleAssembly.GetTypes().Where(t => typeof(IViewFor).IsAssignableFrom(t));
            var viewProvider = TypeService.Resolve<IViewProvider>();
            foreach (var viewType in viewTypes)
            {
                viewProvider.Register((IViewFor)viewType);
            }
        }
    }
}