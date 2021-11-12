using System.Windows;
using System.Windows.Controls;

namespace xFrame.WPF.ViewAdapters;

public class ItemsControlAdapter : BaseViewAdapter<ItemsControl>
{
    public override void InjectView(UIElement element, ItemsControl container)
    {
        InjectView(element, container);
    }

    public override void InjectView(UIElement view, UIElement container)
    {
        if(container is not ItemsControl itemsControl)
        {
            throw new InvalidOperationException("container has to be an ItemsControl");
        }

        itemsControl.Items.Add(view);
    }
}
