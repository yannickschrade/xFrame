using Microsoft.UI.Xaml;
using System;
using xFrame.Core.ViewInjection;

namespace xFrame.WinUI3.ViewAdapters
{
    public abstract class ViewAdapterBase<T> : IViewAdapter
        where T : FrameworkElement
    {
        public Type ViewType => typeof(T);

        public void AddChildToView(object view, object child)
        {

            if (view is not T parentView)
            {
                throw new ArgumentException($"{view} must be an {typeof(T)}");
            }

            if (child is not FrameworkElement uiElement)
            {
                throw new ArgumentException($"{child} must be an {typeof(FrameworkElement)}");
            }

            AddToView(parentView, uiElement);
        }

        public void RemoveChildFromView(object view, object child)
        {
            if (view is not T parentView)
            {
                throw new ArgumentException($"{view} must be an {typeof(T)}");
            }

            if (child is not FrameworkElement uiElement)
            {
                throw new ArgumentException($"{child} must be an {typeof(FrameworkElement)}");
            }

            RemoveFromView(parentView, uiElement);
        }

        public void RemoveAllChilderen(object view)
        {
            if (view is not T parentView)
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
