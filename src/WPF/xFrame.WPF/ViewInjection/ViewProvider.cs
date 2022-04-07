using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using xFrame.Core.ExtensionMethodes;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewInjection
{
    internal class ViewProvider : IViewProvider, IViewRegistration
    {
        private readonly IServiceProvider _services;
        private readonly Dictionary<Type, Type> _viewModelToViewMapping = new Dictionary<Type, Type>();

        public ViewProvider(IServiceProvider services)
        {
            _services = services;
        }

        public FrameworkElement GetView(Type viewType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(viewType))
                throw new ArgumentException($"Type must be an {typeof(FrameworkElement)}");


            var vmType = viewType.GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IViewFor<>))
                .Select(t => t.GetGenericArguments()[0]).FirstOrDefault();

            if(vmType != null)
            {
                return GetViewForViewModel(vmType);
            }
            
            return (FrameworkElement)_services.GetUnregistredService(viewType);
        }

        public T GetView<T>() 
            where T : FrameworkElement
        {
            return (T)GetView(typeof(T));
        }

        public FrameworkElement GetViewForViewModel(IViewModel vm)
        {
            var viewType = _viewModelToViewMapping[vm.GetType()];
            var uiElement = (FrameworkElement)_services.GetUnregistredService(viewType);
            uiElement.Loaded += (s, e) => {
                vm.OnLoaded();
                };
            uiElement.DataContext = vm;
            return uiElement;
        }

        public FrameworkElement GetViewForViewModel(Type viewModelType)
        {
            var vm = (IViewModel)_services.GetUnregistredService(viewModelType);
            return GetViewForViewModel(vm);
        }

        public FrameworkElement GetViewForViewModel<T>()
            where T : IViewModel
        {
            return GetViewForViewModel(typeof(T));
        }

        public void Register(IViewFor view)
        {
            _viewModelToViewMapping.Add(view.ViewModelType, view.GetType());
        }

        public void Register(Type viewType, Type viewModelType)
        {
            _viewModelToViewMapping.Add(viewModelType, viewType);
        }

        public void Register<TView, TViewModel>()
            where TView : FrameworkElement
            where TViewModel : IViewModel
        {
            Register(typeof(TView), typeof(TViewModel));
        }

        public void Register(Type viewType)
        {
            if (!typeof(IViewFor).IsAssignableFrom(viewType))
                throw new ArgumentException($"Type must implement {typeof(IViewFor<>)}");

            var vmType = viewType.GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IViewFor<>))
                .Select(t => t.GetGenericArguments()[0]).FirstOrDefault();
            _viewModelToViewMapping.Add(vmType, viewType);
        }
    }
}
