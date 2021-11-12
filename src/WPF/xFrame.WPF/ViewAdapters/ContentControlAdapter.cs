using System.Windows;
using System.Windows.Controls;

namespace xFrame.WPF.ViewAdapters;

public class ContentControlAdapter : BaseViewAdapter<ContentControl>
{
    public override void InjectView(UIElement element, ContentControl container)
    {
        InjectView(element,container);
    }

    public override void InjectView(UIElement view, UIElement container)
    {
        if(container is not ContentControl contentControl)
        {
            throw new InvalidOperationException("container has to be an ContentControl");
        }

        contentControl.Content = view;
    }
}
