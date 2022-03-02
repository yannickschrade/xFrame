﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using xFrame.Core.IoC;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewInjection
{
    internal class ViewProvider : IViewProvider, IViewRegistration
    {
        private readonly ITypeProviderService _typeProvider;
        private readonly Dictionary<Type, Type> _viewModelToViewMapping = new Dictionary<Type, Type>();

        public ViewProvider(ITypeProviderService typeProvider)
        {
            _typeProvider = typeProvider;
        }

        public FrameworkElement GetView(Type viewType)
        {
            if (!typeof(FrameworkElement).IsAssignableFrom(viewType))
                throw new ArgumentException($"Type must be an {typeof(FrameworkElement)}");

            return (FrameworkElement)_typeProvider.Resolve(viewType);
        }

        public T GetView<T>() 
            where T : FrameworkElement
        {
            return _typeProvider.Resolve<T>();
        }

        public FrameworkElement GetViewForViewModel(IViewModel vm)
        {
            var viewType = _viewModelToViewMapping[vm.GetType()];
            var uiElement = (FrameworkElement)_typeProvider.Resolve(viewType);
            uiElement.Loaded += (s, e) => vm.OnLoaded();
            uiElement.DataContext = vm;
            return uiElement;
        }

        public FrameworkElement GetViewForViewModel(Type viewModelType)
        {
            var vm = (IViewModel)_typeProvider.Resolve(viewModelType);
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
                // TODO : Better excpetion
                throw new Exception();

            var vmType = viewType.GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IViewFor<>)).Select(t => t.GetGenericArguments()[0]).FirstOrDefault();
            _viewModelToViewMapping.Add(vmType, viewType);

        }
    }
}
