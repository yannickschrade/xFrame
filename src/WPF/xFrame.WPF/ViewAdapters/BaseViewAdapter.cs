using System.Windows;
using xFrame.WPF.ViewInjection;

namespace xFrame.WPF.ViewAdapters;

public abstract class BaseViewAdapter<T> : IViewAdapter<T>
    where T : UIElement
{
    public Type ForType => typeof(T);

    public abstract void InjectView(UIElement element, T container);

    public abstract void InjectView(UIElement view, UIElement container);
}
