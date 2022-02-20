using System;
using System.Collections.Generic;
using System.Text;

namespace xFrame.Core.ViewInjection
{
    public interface IViewAdapterCollection
    {
        void RegisterAdapter(IViewAdapter adapter);

        void RegisterAdapterIfMissing(IViewAdapter adapter);

        void RegisterAdapter<T>()
            where T : IViewAdapter, new();
        
        void RegisterAdapterIfMissing<T>()
            where T : IViewAdapter, new();

        IViewAdapter GetAdapterForView(Type viewType);
        IViewAdapter GetAdapterForView<T>();

        IViewAdapter GetAdapterForView(object view);

        void RegisterAdapter(Type adapterType);

        void RegisterAdapters(IEnumerable<Type> adpaters);
    }
}
