using System.Windows;

namespace xFrame.WPF.ViewProvider;

public interface IViewProviderService
{
    T GetView<T>()
        where T : UIElement;

    UIElement GetView(Type viewType);

    T GetWindow<T>()
        where T : Window;

    Window GetWindow(Type windowType);
}
