using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using xFrame.Core.MVVM;
using xFrame.Core.ViewInjection;

namespace xFrame.WinUI3.ViewInjection
{
    internal class ViewContainer : IViewContainer
    {
        private readonly List<FrameworkElement> _attachedViews = new();
        private readonly IViewProvider _viewProvider;
        private readonly Dictionary<IViewModel, FrameworkElement> _injectedViews = new();
        private readonly DispatcherQueue _dispatcher;

        public object Key { get; }
        public IViewAdapterCollection ViewAdapterCollection { get; }

        public ViewContainer(object key, IViewAdapterCollection viewAdapterCollection, IViewProvider viewProvider, DispatcherQueue dispatcher)
        {
            Key = key;
            ViewAdapterCollection = viewAdapterCollection;
            _viewProvider = viewProvider;
            _dispatcher = dispatcher;
        }


        public void AttachTo(object view)
        {
            if (view is not FrameworkElement uiElement)
                throw new ArgumentException($"{view.GetType()} must be an {typeof(FrameworkElement)}");

            _attachedViews.Add(uiElement);
            var adapter = ViewAdapterCollection.GetAdapterForView(view);

            foreach (var childviews in _injectedViews.Values)
            {
                _dispatcher.TryEnqueue(() => adapter.AddChildToView(uiElement, childviews));
            }
        }

        public void RemoveFrom(object view)
        {
            if (view is not FrameworkElement uiElement)
                throw new ArgumentException($"{view.GetType()} must be an {typeof(FrameworkElement)}");

            if (!_attachedViews.Contains(uiElement))
            {
                return;
            }
            _attachedViews.Remove(uiElement);
            var adapter = ViewAdapterCollection.GetAdapterForView(uiElement);
            _dispatcher.TryEnqueue(() => adapter.RemoveAllChilderen(uiElement));
        }

        public void Inject(Type viewModelType)
        {
            var childview = _viewProvider.GetViewWithViewModel(viewModelType);
            Inject(childview);
        }

        public void Inject(IViewModel vm)
        {
            var childview = _viewProvider.GetViewWithViewModel(vm);
            Inject(childview);
        }

        public void Remove(Type viewModelType)
        {

        }

        public void Remove(IViewModel vm)
        {
            if (!_injectedViews.TryGetValue(vm, out var childview))
                return;

            _injectedViews.Remove(vm);

            foreach (var view in _attachedViews)
            {
                var adapter = ViewAdapterCollection.GetAdapterForView(view);
                _dispatcher.TryEnqueue(() => adapter.RemoveChildFromView(view, childview));
            }
        }

        private void Inject(FrameworkElement uIElement)
        {

            var vm = (IViewModel)uIElement.DataContext;
            if (uIElement is IViewFor viewFor)
                vm = viewFor.ViewModel;
            if (!_injectedViews.TryAdd(vm, uIElement))
            {
                // TODO: better exceptions
                throw new Exception("viewmodel allready added");
            }

            foreach (var view in _attachedViews)
            {
                var adapter = ViewAdapterCollection.GetAdapterForView(view);
                _dispatcher.TryEnqueue(() => adapter.AddChildToView(view, uIElement));
            }
        }

        public void InjectView(object view)
        {
            if (view is not FrameworkElement frameworkElement)
                throw new ArgumentException($"view must be an {typeof(FrameworkElement)}");
            Inject(frameworkElement);
        }

        public void InjectView(Type viewType)
        {
            _viewProvider.GetView
        }
    }
}
