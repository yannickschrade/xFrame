using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using xFrame.Core.IoC;
using xFrame.Core.MVVM;

namespace xFrame.WinUI3.ViewInjection
{
    internal class ViewProvider : IViewProvider, IViewRegistration
    {
        private readonly ITypeProviderService _typeProvider;
        private readonly Dictionary<Type, Type> _viewModelToViewMapping = new();
        private readonly Dictionary<Type, Type> _viewModelToWindowMapping = new();

        public ViewProvider(ITypeProviderService typeProvider)
        {
            _typeProvider = typeProvider;
        }

        public FrameworkElement GetViewWithViewModel(IViewModel vm)
        {
            var viewType = _viewModelToViewMapping[vm.GetType()];
            var uiElement = (IViewFor)_typeProvider.Resolve(viewType);
            uiElement.ViewModel = vm;
            return (FrameworkElement)uiElement;
        }

        public FrameworkElement GetViewWithViewModel(Type viewModelType)
        {           
            var vm = (IViewModel)_typeProvider.Resolve(viewModelType);
            return GetViewWithViewModel(vm);
        }

        public FrameworkElement GetViewWithViewModel<T>()
            where T : IViewModel
        {
            return GetViewWithViewModel(typeof(T));
        }

        public Window GetWindowWithViewModel(IViewModel vm)
        {
            var viewType = _viewModelToWindowMapping[vm.GetType()];
            var window = (Window)_typeProvider.Resolve(viewType);
            ((IViewFor)window).ViewModel = vm;
            return window;
        }

        public Window GetWindowWithViewModel(Type viewModelType)
        {
            var vm = (IViewModel)_typeProvider.Resolve(viewModelType);
            return GetWindowWithViewModel(vm);
        }

        public Window GetWindowWithViewModel<T>()
        {
            return GetWindowWithViewModel(typeof(T));
        }

        public void Register(IViewFor view)
        {
            var viewType = view.GetType();
            if (!viewType.IsAssignableTo(typeof(FrameworkElement)))
                throw new ArgumentException(); // TODO: Exception

            Register(viewType, view.ViewModelType);
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
                throw new ArgumentException(); // TODO : Better excpetion

            var vmType = viewType.GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IViewFor<>)).Select(t => t.GetGenericArguments()[0]).FirstOrDefault();
            Register(viewType, vmType);

        }

        public void RegisterWindow<TWindow, TViewModel>()
            where TWindow : Window
            where TViewModel : IViewModel
        {
            Register(typeof(TWindow), typeof(TViewModel));
        }

        public void Register(Type viewType, Type viewModelType)
        {
            if (!viewType.IsAssignableTo(typeof(IViewFor)))
                throw new ArgumentException();

            if (viewType.IsAssignableTo(typeof(Window)))
            {
                _viewModelToWindowMapping.Add(viewModelType, viewType);
                return;
            }

            if (viewType.IsAssignableTo(typeof(FrameworkElement)))
            {
                _viewModelToViewMapping.Add(viewModelType, viewType);
                return;
            }

            throw new ArgumentException(); // TODO: Exception
        }
    }
}
