using System;
using System.Collections.Generic;
using xFrame.Core.MVVM;
using xFrame.Core.ViewService;
using xFrame.WPF.ViewService;

namespace xFrame.WPF.ViewInjection
{
    internal class ViewManager : IViewManager
    {
        public static Dictionary<string,IViewContainer> ViewContainers = new Dictionary<string, IViewContainer>();

        private readonly IViewAdapterCollection _viewAdapters;
        private readonly IViewProviderService _viewProvider;


        public ViewManager(IViewAdapterCollection viewAdapters, IViewProviderService viewProvider)
        {
            _viewAdapters = viewAdapters;
            _viewProvider = viewProvider;
        }

        public IViewContainer this[string key] => ViewContainers[key];

        public void AddToContainer(ViewModelBase viewModel, string containerKey)
        {
            var view = _viewProvider.GetView(viewModel);
            var containerType = ViewContainers[containerKey].TargetView.GetType();
            var adapter = GetAdapter(containerType);
            ViewContainers[containerKey].Add(view, adapter);
        }

        public void AddToContainer(Type viewModelType, string containerKey)
        {
            var view = _viewProvider.GetView(viewModelType);
            var containerType = ViewContainers[containerKey].TargetView.GetType();
            var adapter = GetAdapter(containerType);
            ViewContainers[containerKey].Add(view,adapter);
        }

        public void AddToContainer<T>(string containerKey)
        {
            AddToContainer(typeof(T), containerKey);
        }

        public void RemoveFromContainer(ViewModelBase viewModel, string containerKey)
        {
            var view = _viewProvider.GetView(viewModel);
            var containerType = ViewContainers[containerKey].TargetView.GetType();
            var adapter = GetAdapter(containerType);
            ViewContainers[containerKey].Remove(view, adapter);
            _viewProvider.RemoveView(viewModel);
        }


        private IViewAdapter GetAdapter(Type forType)
        {
            var basetype = forType;
            while (basetype != null)
            {
                if (_viewAdapters.ContainsAdapterFor(basetype))
                {
                    return _viewAdapters.GetAdapterFor(basetype);
                }
                basetype = basetype.BaseType;
            }

            throw new KeyNotFoundException();
        }
    }
}
