using System.Windows;
using System.Windows.Controls;

namespace xFrame.WPF.ViewAdapters;

public class PanelAdapter : BaseViewAdapter<Panel>
{
    public override void InjectView(UIElement element, Panel container)
    {
        container.Children.Add(element);
    }
}
