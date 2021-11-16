using System;
using System.Collections.Generic;
using xFrame.Core.IoC;
using xFrame.Core.ViewService;

namespace xFrame.WPF.ViewInjection
{
    public class ViewAdapterCollection : IViewAdapterCollection
    {
        private readonly ITypeProviderService _typeProvider;
        private readonly Dictionary<Type, IViewAdapter> _viewAdapters = new Dictionary<Type, IViewAdapter>();

        public ViewAdapterCollection(ITypeProviderService typeProviderService)
        {
            _typeProvider = typeProviderService;
        }

        public void AddAdapter(IViewAdapter adapter)
        {
            _viewAdapters[adapter.ForType] = adapter;
        }

        public void AddAdapterIfMissing(IViewAdapter adapter)
        {
            if (_viewAdapters.ContainsKey(adapter.ForType))
            {
                return;
            }
            _viewAdapters.Add(adapter.ForType, adapter);
        }

        public void RegisterAdapter(Type adapterType)
        {
            var adapter = CreateAdapter(adapterType);
            _viewAdapters[adapter.ForType] = adapter;
        }

        public void RegisterAdapter<T>() where T : IViewAdapter, new()
        {
            RegisterAdapter(typeof(T));
        }

        public void RegisterAdapterIfMissing(Type adapterType)
        {
            var adapter = CreateAdapter(adapterType);
            if (!_viewAdapters.ContainsKey(adapterType))
            {
                _viewAdapters.Add(adapter.ForType, adapter);
            }
        }

        public void RegisterAdapterIfMissing<T>() where T : IViewAdapter, new()
        {
            RegisterAdapterIfMissing(typeof(T));
        }

        public bool ContainsAdapterFor(Type controlType)
        {
            return _viewAdapters.ContainsKey(controlType);
        }

        public bool ContainsAdapterFor<T>()
        {
            return _viewAdapters.ContainsKey(typeof(T));
        }

        public IViewAdapter GetAdapterFor(Type controlType)
        {
            return _viewAdapters[controlType];
        }

        public IViewAdapter GetAdapterFor<T>() where T : class
        {
            return _viewAdapters[typeof(T)];
        }

        private IViewAdapter CreateAdapter(Type adapterType)
        {
            var adapter = _typeProvider.Resolve(adapterType);
            if(adapter is IViewAdapter viewAdapter)
            {
                return viewAdapter;
            }
            throw new Exception();
        }

        public void RegisterAdapters(IEnumerable<Type> adapterTypes)
        {
            foreach (var adapterType in adapterTypes)
            {
                RegisterAdapter(adapterType);
            }
        }

        public void RegisterAdaptersIfMissing(IEnumerable<Type> adaptertypes)
        {
            foreach (var adapterType in adaptertypes)
            {
                RegisterAdapterIfMissing(adapterType);
            }
        }
    }
}