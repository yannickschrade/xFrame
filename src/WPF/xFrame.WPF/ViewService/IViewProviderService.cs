using System.Windows;

namespace xFrame.WPF.ViewService;

public interface IViewProviderService
{
    T GetView<T>()
        where T : UIElement;

    UIElement GetView(Type viewType);

    T CreateWindow<T>()
        where T : Window;

    Window CreateWindow(Type windowType);
}
