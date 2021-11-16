using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using xFrame.Core.IoC;
using xFrame.Core.MVVM;
using xFrame.Core.ViewService;

namespace xFrame.WPF.ViewService
{
    internal class ViewRegistry : IViewRegistry
    {

        private readonly Dictionary<ViewModelBase, IViewFor> _viewRegistry = new Dictionary<ViewModelBase, IViewFor>();
        private readonly Dictionary<Type, Type> _registeredViews = new Dictionary<Type, Type>();
        private readonly ITypeProviderService _typeprovider;

        public ViewRegistry(ITypeProviderService typeProvider)
        {
            _typeprovider = typeProvider;
        }

        public void RegisterView(Type viewType)
        {
            if (!typeof(IViewFor).IsAssignableFrom(viewType))
            {
                throw new ArgumentException("ViewType has to implement the IViewFor<> interface", nameof(viewType));
            }

            if (!viewType.IsClass || viewType.IsAbstract)
            {
                throw new ArgumentException("view type have to be creatable", nameof(viewType));
            }
            var vmType = viewType.GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IViewFor<>)).Select(t => t.GetGenericArguments()[0]).FirstOrDefault();
            _registeredViews.Add(vmType, viewType);
        }

        public void RegisterView<T>()
            where T : UIElement, IViewFor, new()
        {
            RegisterView(typeof(T));
        }

        public IViewFor GetView(ViewModelBase viewModel)
        {
            if (_viewRegistry.TryGetValue(viewModel, out var view))
            {
                return view;
            }
            view = CreateView(_registeredViews[viewModel.GetType()]);
            view.DataContext = viewModel;
            _viewRegistry.Add(viewModel, view);
            return view;
        }

        public IViewFor GetView(Type viewModelType)
        {
            if (!typeof(ViewModelBase).IsAssignableFrom(viewModelType))
            {
                throw new ArgumentException("ViewType has to implement the IViewFor interface", nameof(viewModelType));
            }

            if (!viewModelType.IsClass || viewModelType.IsAbstract)
            {
                throw new ArgumentException("view type have to be creatable", nameof(viewModelType));
            }

            var view = CreateView(viewModelType);
            var vm = (ViewModelBase)_typeprovider.Resolve(viewModelType);
            view.DataContext = vm;
            _viewRegistry.Add(vm, view);

            return view;
        }

        public IViewFor GetView<T>()
            where T : ViewModelBase
        {
            return GetView(typeof(T));
        }

        private IViewFor CreateView(Type viewModelType)
        {

            return (IViewFor)_typeprovider.Resolve(_registeredViews[viewModelType]);
        }
    }
}
