using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using xFrame.Core.IoC;
using xFrame.Core.MVVM;
using xFrame.Core.ViewInjection;

namespace xFrame.WinUI3.ViewInjection
{
    public class ViewInjectionService : IViewInjectionService
    {
        public static readonly Dictionary<object, IViewContainer> Containers = new Dictionary<object, IViewContainer>();
        private readonly IViewProvider _viewProvider;
        private readonly IViewAdapterCollection _viewAdapterCollection;
        private readonly DispatcherQueue _dispatcher;

        #region UI

        public static readonly DependencyProperty ContainerKeyProperty = DependencyProperty.RegisterAttached(
            "ContainerKey",
            typeof(object),
            typeof(ViewInjectionService),
            new PropertyMetadata(null, OnContainerKeySet));

        public static void SetContainerKey(DependencyObject target, object key)
        {
            target.SetValue(ContainerKeyProperty, key);
        }

        public static object GetContainerKey(DependencyObject target)
        {
            return target.GetValue(ContainerKeyProperty);
        }

        private static void OnContainerKeySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!Containers.ContainsKey(e.NewValue))
            {
                var adaptercollection = TypeProvider.Current.Resolve<IViewAdapterCollection>();
                var viewProvider = TypeProvider.Current.Resolve<IViewProvider>();
                var dispatcher = TypeProvider.Current.Resolve<DispatcherQueue>();
                Containers[e.NewValue] = new ViewContainer(e.NewValue, adaptercollection, viewProvider, dispatcher);
            }

            Containers[e.NewValue].AttachTo(d);
        }

        #endregion


        public ViewInjectionService(IViewProvider viewProvider, IViewAdapterCollection viewAdapterCollection, DispatcherQueue dispatcher)
        {
            _viewProvider = viewProvider;
            _viewAdapterCollection = viewAdapterCollection;
            _dispatcher = dispatcher;
        }

        public void Inject(Type viewModelType, object key)
        {
            if (!Containers.ContainsKey(key))
                Containers[key] = new ViewContainer(key, _viewAdapterCollection, _viewProvider, _dispatcher);
            Containers[key].Inject(viewModelType);
        }

        public void Inject(ViewModelBase vm, object key)
        {
            if (!Containers.ContainsKey(key))
                Containers[key] = new ViewContainer(key, _viewAdapterCollection, _viewProvider, _dispatcher);
            Containers[key].Inject(vm);
        }

        public void Inject<T>(object key) where T : ViewModelBase
        {
            Inject(typeof(T), key);
        }

        public void Remove(ViewModelBase vm, object key)
        {
            Containers[key]?.Remove(vm);
        }

        public void AttachContainer(object view, object key)
        {
            if (view is not FrameworkElement uiElement)
                throw new ArgumentException();

            if (!Containers.ContainsKey(key))
                Containers[key] = new ViewContainer(key, _viewAdapterCollection, _viewProvider, _dispatcher);
            Containers[key].AttachTo(uiElement);

        }
    }
}
