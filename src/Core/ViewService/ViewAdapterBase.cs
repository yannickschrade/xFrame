using System;

namespace xFrame.Core.ViewService
{
    public abstract class ViewAdapterBase<T> : IViewAdapter
    {
        public Type ForType => typeof(T);

        public abstract void Add(object view, T container);
        public abstract void Remove(object view, T container);
        public abstract void NavigateTo(object view, T container);

        public void Add(object view, object container)
        {
            Add(view,(T)container);
        }

        public void NavigateTo(object view, object container)
        {
            NavigateTo(view,(T)container);
        }

        public void Remove(object view, object container)
        {
            Remove(view, (T)container);
        }
    }
}
