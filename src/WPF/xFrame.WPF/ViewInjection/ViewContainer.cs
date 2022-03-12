using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Windows;
using xFrame.Core.MVVM;
using xFrame.Core.ViewInjection;

namespace xFrame.WPF.ViewInjection
{
    internal class ViewContainer : IViewContainer
    {
        private readonly List<UIElement> _attachedViews = new List<UIElement>();
        private readonly IViewProvider _viewProvider;
        private readonly Dictionary<object, FrameworkElement> _injectedViews = new Dictionary<object, FrameworkElement>();

        public object Key { get; }
        public IViewAdapterCollection ViewAdapterCollection { get; }

        public ViewContainer(object key, IViewAdapterCollection viewAdapterCollection, IViewProvider viewProvider)
        {
            Key = key;
            ViewAdapterCollection = viewAdapterCollection;
            _viewProvider = viewProvider;
        }


        public void AttachTo(object view)
        {
            if (!(view is FrameworkElement uiElement))
                throw new ArgumentException($"{view.GetType()} must be an {typeof(FrameworkElement)}");

            _attachedViews.Add(uiElement);
            var adapter = ViewAdapterCollection.GetAdapterForView(view);

            foreach (var childviews in _injectedViews.Values)
            {
                adapter.AddChildToView(uiElement, childviews);
            }
        }

        public void RemoveFrom(object view)
        {
            if (!(view is FrameworkElement uiElement))
                throw new ArgumentException($"{view.GetType()} must be an {typeof(FrameworkElement)}");

            if (!_attachedViews.Contains(uiElement))
            {
                return;
            }
            _attachedViews.Remove(uiElement);
            var adapter = ViewAdapterCollection.GetAdapterForView(uiElement);
            adapter.RemoveAllChilderen(uiElement);
        }

        public void Inject(Type viewModelType)
        {
            var childview = _viewProvider.GetViewForViewModel(viewModelType);
            Inject(childview);
        }

        public void Inject(IViewModel vm)
        {
            var childview = _viewProvider.GetViewForViewModel(vm);
            Inject(childview);
        }

        public void InjectView(Type viewType)
        {
            var view = _viewProvider.GetView(viewType);
            Inject(view);
        }

        public void InjectView(object view)
        {
            if (!(view is FrameworkElement uiElement))
                throw new ArgumentException($"view must be an {typeof(FrameworkElement)}");
            Inject(uiElement);
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
                adapter.RemoveChildFromView(view, childview);

            }
        }

        private void Inject(FrameworkElement uIElement)
        {
            var key = uIElement.DataContext == null ? uIElement : uIElement.DataContext;
            if (!_injectedViews.TryAdd(key, uIElement))
            {
                // TODO: better exceptions
                throw new Exception("viewmodel allready added");
            }

            foreach (var view in _attachedViews)
            {
                var adapter = ViewAdapterCollection.GetAdapterForView(view);
                adapter.AddChildToView(view, uIElement);
            }
        }
    }
}
