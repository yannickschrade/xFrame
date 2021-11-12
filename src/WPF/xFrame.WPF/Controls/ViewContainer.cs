using System.Windows.Controls;
using System.Windows;

namespace xFrame.WPF.Controls;

public class ViewContainer : ContentControl
{
    public static readonly Dictionary<string, UIElement> ExsitingContainers = new();



    public string Key
    {
        get { return (string)GetValue(KeyProperty); }
        set { SetValue(KeyProperty, value); }
    }

    public static readonly DependencyProperty KeyProperty =
        DependencyProperty.Register(nameof(Key), typeof(string), typeof(ViewContainer), new PropertyMetadata(null));


    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if(Key is null)
        {
            throw new ArgumentNullException(nameof(Key));
        }

        if(Content is null)
        {
            ExsitingContainers.Add(Key, this);
            return;
        }

        if(Content is not UIElement uiElement)
        {
            throw new ArgumentException("Content has to be an UIElement");
        }

        ExsitingContainers[Key] = uiElement;

    }
}
