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

    void InjectView<TView>(string viewContainerKey);

    void InjectView(Type viewType, string viewContainerKey);

    void InjectView(UIElement view, string viewContainerKey);

    void Initialize();
}
