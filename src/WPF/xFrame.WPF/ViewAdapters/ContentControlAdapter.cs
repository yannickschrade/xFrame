using System.Windows;
using System.Windows.Controls;

namespace xFrame.WPF.ViewAdapters;

public class ContentControlAdapter : BaseViewAdapter<ContentControl>
{
    public override void InjectView(UIElement view, ContentControl container)
    {
        container.Content = view;
    }

}
