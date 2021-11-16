using System;
using System.Collections.Generic;
using xFrame.Core.ViewService;

namespace xFrame.WPF.ViewInjection
{
    public interface IViewAdapterCollection
    {
        void AddAdapter(IViewAdapter adapter);

        void AddAdapterIfMissing(IViewAdapter adapter);

        void RegisterAdapter(Type adapterType);
        void RegisterAdapters(IEnumerable<Type> adapterTypes);

        void RegisterAdapterIfMissing(Type adapterType);

        void RegisterAdaptersIfMissing(IEnumerable<Type> adaptertypes);

        void RegisterAdapter<T>()
            where T : IViewAdapter, new();

        void RegisterAdapterIfMissing<T>()
            where T : IViewAdapter, new();

        IViewAdapter GetAdapterFor(Type controlType);
        IViewAdapter GetAdapterFor<T>()
            where T : class;

        bool ContainsAdapterFor(Type controlType);
        bool ContainsAdapterFor<T>();
    }
}