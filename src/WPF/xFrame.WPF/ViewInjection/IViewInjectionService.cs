using System.Windows;
using xFrame.WPF.Controls;

namespace xFrame.WPF.ViewInjection;

public interface IViewInjectionService
{
    bool IsInitialized { get; }

    void AddViewAdapter(IViewAdapter adapter);

    void AddViewAdapter<T>(IViewAdapter<T> adapter)
        where T : UIElement;

    void AddViewAdapters(IEnumerable<IViewAdapter> adapters);

    void InjectView<TView>(object containerKey);

    void InjectView(Type viewType, object containerKey);

    void InjectView(UIElement view, object containerKey);

    void Initialize();
}
