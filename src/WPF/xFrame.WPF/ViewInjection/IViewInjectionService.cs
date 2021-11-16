using System.Windows;
using xFrame.WPF.Controls;

namespace xFrame.WPF.ViewInjection;

public interface IViewInjectionService
{
    bool IsInitialized { get; }

    void AddAdapter(IViewAdapter adapter);

    void AddAdapterIfMissing(IViewAdapter adapter);

    void AddAdapter<T>(IViewAdapter<T> adapter)
        where T : UIElement;


    void RegisterAdapter(Type adapterType);

    void RegisterAdapterIfMissing(Type adapterType);

    void RegisterAdapters(IEnumerable<Type> adapterTypes);

    void RegisterAdaptersIfMissing(IEnumerable<Type> adapterTypes);

    void RegisterAdapter<T>()
        where T : IViewAdapter, new();

    void RegisterAdapterIfMissing<T>()
        where T : IViewAdapter, new();

    void AddAdapters(IEnumerable<IViewAdapter> adapters);

    void AddAdaptersIfMissing(IEnumerable<IViewAdapter> adapters);

    void InjectView<TView>(object containerKey);

    void InjectView(Type viewType, object containerKey);

    void InjectView(UIElement view, object containerKey);

    void Initialize();
}
