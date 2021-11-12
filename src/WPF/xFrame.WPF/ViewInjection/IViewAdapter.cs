using System.Configuration;
using System.Windows;

namespace xFrame.WPF.ViewInjection
{
    public interface IViewAdapter
    {
        Type ForType { get; }

        void InjectView(UIElement view, UIElement container);
    }

    public interface IViewAdapter<TForContainer> : IViewAdapter
        where TForContainer : UIElement
    {
        void InjectView(UIElement uIElement, TForContainer container);
    }
}