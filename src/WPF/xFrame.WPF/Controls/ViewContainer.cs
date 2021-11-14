using System.Windows.Controls;
using System.Windows;
using xFrame.WPF.ViewInjection;

namespace xFrame.WPF.Controls;

public class ViewContainer : ContentControl
{
    public object Key { get; set; }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if(Key is null)
        {
            throw new ArgumentNullException(nameof(Key));
        }

        if(Content is null)
        {
            ViewInjector.ExsitingContainers.Add(Key, this);
            return;
        }

        if(Content is not UIElement uiElement)
        {
            throw new ArgumentException("Content has to be an UIElement");
        }

        ViewInjector.ExsitingContainers[Key] = uiElement;

    }
}
