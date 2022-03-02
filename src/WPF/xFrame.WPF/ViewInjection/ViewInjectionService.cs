using System;
using System.Collections.Generic;
using System.Windows;
using xFrame.Core.IoC;
using xFrame.Core.MVVM;
using xFrame.Core.ViewInjection;

namespace xFrame.WPF.ViewInjection
{
    public class ViewInjectionService : IViewInjectionService
    {
        public static readonly Dictionary<object, IViewContainer> Containers = new Dictionary<object, IViewContainer>();
        private readonly IViewProvider _viewProvider;
        private readonly IViewAdapterCollection _viewAdapterCollection;

        #region WPF

        public static readonly DependencyProperty ContainerKeyProperty = DependencyProperty.RegisterAttached(
            "ContainerKey",
            typeof(object),
            typeof(ViewInjectionService),
            new PropertyMetadata(null, OnContainerKeySet));

        public static void SetContainerKey(DependencyObject target, object key)
        {
            target.SetValue(ContainerKeyProperty, key);
        }

        private static void OnContainerKeySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!Containers.ContainsKey(e.NewValue))
            {
                var adaptercollection = TypeService.Current.Resolve<IViewAdapterCollection>();
                var viewProvider = TypeService.Current.Resolve<IViewProvider>();
                Containers[e.NewValue] = new ViewContainer(e.NewValue, adaptercollection, viewProvider);
            }

            Containers[e.NewValue].AttachTo(d);
        }

        #endregion


        public ViewInjectionService(IViewProvider viewProvider, IViewAdapterCollection viewAdapterCollection)
        {
            _viewProvider = viewProvider;
            _viewAdapterCollection = viewAdapterCollection;
        }

        public void Inject(Type viewModelType, object key)
        {
            if (!Containers.ContainsKey(key))
                Containers[key] = new ViewContainer(key, _viewAdapterCollection, _viewProvider);
            Containers[key].Inject(viewModelType);
        }

        public void Inject(IViewModel vm, object key)
        {
            if (!Containers.ContainsKey(key))
                Containers[key] = new ViewContainer(key, _viewAdapterCollection, _viewProvider);
            Containers[key].Inject(vm);
        }

        public void Inject<T>(object key) where T : IViewModel
        {
            Inject(typeof(T), key);
        }

        public void InjectView(Type viewType, object key)
        {
            if (!Containers.ContainsKey(key))
                Containers[key] = new ViewContainer(key, _viewAdapterCollection, _viewProvider);
            Containers[key].InjectView(viewType);
        }

        public void InjectView<T>(object key)
        {
            InjectView(typeof(T), key);
        }

        public void Remove(IViewModel vm, object key)
        {
            Containers[key]?.Remove(vm);
        }

        public void AttachContainer(object view, object key)
        {
            if (!(view is FrameworkElement uiElement))
                throw new ArgumentException();

            if (!Containers.ContainsKey(key))
                Containers[key] = new ViewContainer(key, _viewAdapterCollection, _viewProvider);
            Containers[key].AttachTo(uiElement);

        }
    }
}
