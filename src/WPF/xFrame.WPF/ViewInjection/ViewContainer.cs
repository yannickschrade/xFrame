using System;
using System.Collections.Generic;
using System.Windows;
using xFrame.Core.ViewService;

namespace xFrame.WPF.ViewInjection
{
    public class ViewContainer : IViewContainer
    {

        public static readonly DependencyProperty KeyProperty = DependencyProperty.RegisterAttached(
            nameof(KeyProperty), 
            typeof(string),
            typeof(ViewContainer),
            new PropertyMetadata(null, OnContainerKeySet));

        private static void OnContainerKeySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var key = e.NewValue as string;
            var container = new ViewContainer(d);
            ViewManager.ViewContainers.Add(key, container);
            var element = d as FrameworkElement;

            if(element == null)
            {
                throw new NotSupportedException("The dependency object must be an frameworkelement");
            }

            element.Loaded += (s, e) => container.Initialize();
        }

        public static void SetKey(DependencyObject target, string key)
        {
            target.SetValue(KeyProperty, key);
        }

        public static string GetKey(DependencyObject target)
        {
            return target.GetValue(KeyProperty) as string;
        }


        private readonly Queue<(IViewFor view, IViewAdapter adapter)> _addQueue = new Queue<(IViewFor view, IViewAdapter adapter)>();
        public object TargetView { get; }
        public bool IsLoaded { get; private set;}


        public ViewContainer(object targetView)
        {
            TargetView = targetView;
        }

        public void Add(IViewFor view, IViewAdapter adapter)
        {
            if (!IsLoaded)
            {
                _addQueue.Enqueue((view, adapter));
                return;
            }

            adapter.Add(view, TargetView);
        }

        public void Remove(IViewFor view, IViewAdapter viewAdapter)
        {
            viewAdapter.Remove(view, TargetView);
        }

        public void Initialize()
        {
            if (IsLoaded)
            {
                return;
            }
            IsLoaded = true;
            foreach (var (view, adapter) in _addQueue)
            {
                Add(view, adapter);
            }
            
        }

        
    }
}
