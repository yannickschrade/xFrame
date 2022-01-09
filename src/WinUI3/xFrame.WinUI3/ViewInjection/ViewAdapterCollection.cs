using System;
using System.Collections.Generic;
using xFrame.Core.IoC;
using xFrame.Core.ViewInjection;

namespace xFrame.WinUI3.ViewInjection
{
    internal class ViewAdapterCollection : IViewAdapterCollection
    {
        private Dictionary<Type, IViewAdapter> _viewAdapters = new Dictionary<Type, IViewAdapter>();

        public IViewAdapter GetAdapterForView(Type viewType)
        {
            var basetype = viewType;
            while (basetype != null)
            {
                if (_viewAdapters.TryGetValue(basetype, out var adapter))
                {
                    return adapter;
                }
                basetype = basetype.BaseType;
            }

            throw new KeyNotFoundException();
        }

        public IViewAdapter GetAdapterForView<T>()
        {
            return GetAdapterForView(typeof(T));
        }

        public IViewAdapter GetAdapterForView(object view)
        {
            return GetAdapterForView(view.GetType());
        }


        public void RegisterAdapter(IViewAdapter adapter)
        {
            _viewAdapters[adapter.ViewType] = adapter;
        }

        public void RegisterAdapter<T>() where T : IViewAdapter, new()
        {
            RegisterAdapter(new T());
        }

        public void RegisterAdapter(Type adapterType)
        {
            if (!typeof(IViewAdapter).IsAssignableFrom(adapterType))
                throw new Exception("no adapter");

            var adapter = (IViewAdapter)Activator.CreateInstance(adapterType);

            RegisterAdapter(adapter);
        }

        public void RegisterAdapterIfMissing(IViewAdapter adapter)
        {
            _viewAdapters.TryAdd(adapter.ViewType, adapter);
        }

        public void RegisterAdapterIfMissing<T>() where T : IViewAdapter, new()
        {
            RegisterAdapterIfMissing(new T());
        }

        public void RegisterAdapters(IEnumerable<Type> adapterTypes)
        {
            foreach (var adapterType in adapterTypes)
            {
                RegisterAdapter(adapterType);
            }
        }
    }
}
