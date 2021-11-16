using System.Windows;
using xFrame.Core.IoC;
using xFrame.WPF.ViewService;

namespace xFrame.WPF.ViewInjection;

public class ViewInjectionService : IViewInjectionService
{
    private readonly IViewProviderService _viewResolver;
    private readonly ITypeProviderService _typeProvider;
    private readonly Queue<(Type viewType, object containerKey)> _injectionQueue = new();
    private readonly Dictionary<Type, IViewAdapter> _viewAdapters = new();


    public bool IsInitialized { get; private set; }

    public ViewInjectionService(IViewProviderService resolver, ITypeProviderService typeProvider)
    {
        _viewResolver = resolver;
        _typeProvider = typeProvider;
    }

    public void AddAdapter<T>(IViewAdapter<T> adapter) where T : UIElement
    {
        AddAdapter(adapter);
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
        AddAdapter(adapter);
    }

    public void AddAdapters(IEnumerable<IViewAdapter> adapters)
    {
        foreach (var adapter in adapters)
        {
            AddAdapter(adapter);
        }
    }

    public void AddAdaptersIfMissing(IEnumerable<IViewAdapter> adapters)
    {
        foreach (var adapter in adapters)
        {
            AddAdapterIfMissing(adapter);
        }
    }


    public void RegisterAdapter<TAdapter>()
        where TAdapter : IViewAdapter, new()
    {
        AddAdapter(_typeProvider.Resolve<TAdapter>());
    }



    public void RegisterAdapter(Type adapterType)
    {
        if (adapterType.IsAssignableTo(typeof(IViewAdapter)))
        {
            // TODO: custom exception
            throw new Exception();
        }

        AddAdapter((IViewAdapter)_typeProvider.Resolve(adapterType));
    }

    public void RegisterAdapterIfMissing(Type adapterType)
    {
        if (!adapterType.IsAssignableTo(typeof(IViewAdapter)))
        {
            // TODO: custom exception
            throw new Exception();
        }
        AddAdapterIfMissing((IViewAdapter)_typeProvider.Resolve(adapterType));
    }

    public void RegisterAdapterIfMissing<T>()
        where T : IViewAdapter, new()
    {
        RegisterAdapterIfMissing(typeof(T));
    }

    public void RegisterAdapters(IEnumerable<Type> adapterTypes)
    {
        foreach (var adaptertype in adapterTypes)
        {
            RegisterAdapter(adaptertype);
        }
    }

    public void RegisterAdaptersIfMissing(IEnumerable<Type> adapterTypes)
    {
        foreach (var adaptertype in adapterTypes)
        {
            RegisterAdapterIfMissing(adaptertype);
        }
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
