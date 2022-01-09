using System;
using System.Windows;
using xFrame.Core.ViewInjection;

namespace xFrame.WPF.ViewAdapters
{
    public abstract class ViewAdapterBase<T> : IViewAdapter
        where T : FrameworkElement
    {
        public Type ViewType => typeof(T);

        public void AddChildToView(object view, object child)
        {

            if (!(view is T parentView))
            {
                throw new ArgumentException($"{view} must be an {typeof(T)}");
            }

            if (!(child is FrameworkElement uiElement))
            {
                throw new ArgumentException($"{child} must be an {typeof(FrameworkElement)}");
            }
            Application.Current.Dispatcher.Invoke(() => AddToView(parentView, uiElement));
        }

        public void RemoveChildFromView(object view, object child)
        {
            if(!(view is T parentView))
            {
                throw new ArgumentException($"{view} must be an {typeof(T)}");
            }

            if (!(child is FrameworkElement uiElement))
            {
                throw new ArgumentException($"{child} must be an {typeof(FrameworkElement)}");
            }

            Application.Current.Dispatcher.Invoke(() => RemoveFromView(parentView, uiElement));
        }

        public void RemoveAllChilderen(object view)
        {
            if (!(view is T parentView))
            {
                throw new ArgumentException($"{view} must be an {typeof(T)}");
            }

            RemoveAllChilderen(parentView);
        }

        public abstract void AddToView(T View, FrameworkElement child);

        public abstract void RemoveFromView(T View, FrameworkElement child);

        public abstract void RemoveAllChildren(T View);
       
    }
}
