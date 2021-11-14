using System.Windows;
using xFrame.WPF.ViewProvider;

namespace xFrame.WPF.ViewInjection;

public class ViewInjectionService : IViewInjectionService
{
    private readonly IViewProviderService _viewResolver;
    private readonly Queue<(Type viewType, object containerKey)> _injectionQueue = new();
    private readonly Dictionary<Type, IViewAdapter> _viewAdapters = new();


    public bool IsInitialized { get; private set; }

    public ViewInjectionService(IViewProviderService resolver)
    {
        _viewResolver = resolver;
    }

    public void AddViewAdapter<T>(IViewAdapter<T> adapter) where T : UIElement
    {
        AddViewAdapter(adapter);
    }

    public void AddViewAdapters(IEnumerable<IViewAdapter> adapters)
    {
        foreach (var adapter in adapters)
        {
            AddViewAdapter(adapter);
        }
    }

    public void AddViewAdapter(IViewAdapter adapter)
    {
        if (_viewAdapters.ContainsKey(adapter.ForType))
        {
            // TODO: Add CustomException
            throw new InvalidOperationException($"Adapter for type: {adapter.ForType} allready exsists");
        }

        _viewAdapters.Add(adapter.ForType, adapter);
    }

    public void InjectView<TView>(object containerKey)
    {
        InjectView(typeof(TView), containerKey);
    }

    public void InjectView(Type viewType, object containerKey)
    {
        var view = _viewResolver.GetView(viewType);
        InjectView(view, containerKey);
    }

    public void InjectView(UIElement view, object containerKey)
    {
        if (!IsInitialized)
        {
            _injectionQueue.Enqueue((view.GetType(), containerKey));
            return;
        }
        if (!ViewInjector.ExsitingContainers.TryGetValue(containerKey, out var container))
        {
            // TODO: Add CustomException
            throw new InvalidOperationException();
        }

        var adapter = GetAdapter(container.GetType());
        adapter.InjectView(view, container);
    }

    public void Initialize()
    {
        IsInitialized = true;
        InjectQueuedViews();
    }

    private void InjectQueuedViews()
    {
        foreach (var (viewType, containerKey) in _injectionQueue)
        {
            InjectView(viewType, containerKey);
        }
    }

    private IViewAdapter GetAdapter(Type controlType)
    {
        var current = controlType;
        while (current != null)
        {
            if (_viewAdapters.ContainsKey(current))
            {
                return _viewAdapters[current];
            }
            current = current.BaseType;
        }
        // TODO: custom exception
        throw new KeyNotFoundException();
    }
}
