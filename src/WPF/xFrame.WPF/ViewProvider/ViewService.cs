using System.Windows;
using xFrame.Core.IoC;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewProvider;

public class ViewService : IViewProviderService, IViewRegistrationService
{
    private readonly Dictionary<Type, Type?> _registeredViews = new();
    private readonly Dictionary<Type, Type?> _registeredWindows = new();
    private readonly Dictionary<Type, UIElement> _views = new();

    private readonly ITypeProviderService _typeProvider;

    public ViewService(ITypeProviderService typeProvider)
    {
        ArgumentNullException.ThrowIfNull(typeProvider, nameof(typeProvider));

        _typeProvider = typeProvider;
    }

    public T GetView<T>() where T : UIElement
    {
        return (T)GetView(typeof(T));
    }

    public UIElement GetView(Type viewType)
    {
        if (!_registeredViews.TryGetValue(viewType, out var viewModelType))
        {
            //TODO: custom exception
            throw new InvalidOperationException();
        }

        if (_views.TryGetValue(viewType, out var view))
        {
            return view;
        }

        view = CreateView(viewType, viewModelType);
        _views.Add(viewType, view);
        return view;
    }

    public T GetWindow<T>() where T : Window
    {
        return (T)GetWindow(typeof(T));
    }

    public Window GetWindow(Type windowType)
    {
        if(!_registeredWindows.TryGetValue(windowType, out var viewModelType))
        {
            //TODO: custom exception
            throw new InvalidOperationException();
        }

        return (Window)CreateView(windowType, viewModelType);
    }


    public IViewRegistrationService RegisterViewWithViewModel(Type viewType, Type viewModelType)
    {
        if (!viewType.IsAssignableTo(typeof(UIElement)))
        {
            //TODO custom exception.
            throw new InvalidOperationException();
        }

        if (!viewModelType.IsAssignableTo(typeof(ViewModelBase)))
        {
            //TODO custom exception.
            throw new InvalidOperationException();
        }

        _registeredViews.Add(viewType, viewModelType);
        return this;
    }

    public IViewRegistrationService RegisterView(Type viewType)
    {
        if (!viewType.IsAssignableTo(typeof(UIElement)))
        {
            //TODO custom exception.
            throw new InvalidOperationException();
        }
        _registeredViews.Add(viewType, null);
        return this;
    }


    public IViewRegistrationService RegisterWindow(Type windowType)
    {
        if (!windowType.IsAssignableTo(typeof(Window)))
        {
            //TODO custom exception.
            throw new InvalidOperationException();
        }

        _registeredWindows.Add(windowType, null);
        return this;
    }

    public IViewRegistrationService RegisterWindowWithViewModel(Type windowType, Type viewModelType)
    {
        if (!windowType.IsAssignableTo(typeof(Window)))
        {
            //TODO custom exception.
            throw new InvalidOperationException();
        }

        if (!viewModelType.IsAssignableTo(typeof(ViewModelBase)))
        {
            //TODO custom exception.
            throw new InvalidOperationException();
        }

        _registeredWindows.Add(windowType, viewModelType);
        return this;
    }

    private UIElement CreateView(Type viewType, Type? viewModelType)
    {
        if(viewModelType is null)
        {
            viewModelType = viewType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IViewFor<>))
                .FirstOrDefault()?
                .GetGenericArguments()
                .First();
        }

        var view = (UIElement)_typeProvider.Resolve(viewType);
        if (viewModelType is not null)
        {
            var vm = _typeProvider.Resolve(viewModelType);
            ((FrameworkElement)view).DataContext = vm;
        }



        return view;
    }
}
