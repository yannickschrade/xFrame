using System;

namespace xFrame.Core.ViewInjection
{
    public interface IViewAdapter
    {
        Type ViewType { get; }

        void AddChildToView(object view, object child);

        void RemoveChildFromView(object view, object child);

        void RemoveAllChilderen(object view);
    }
}
