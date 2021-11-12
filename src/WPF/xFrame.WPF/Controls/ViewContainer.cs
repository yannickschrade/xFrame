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


    protected override void OnContentChanged(object oldContent, object newContent)
    {
        if(string.IsNullOrWhiteSpace(Key)) return;

        if(newContent is null)
        {
            ExsitingContainers[Key] = new ContentControl();
        }

        if(newContent is not UIElement element)
        {
            throw new InvalidOperationException("Content elements must be an UIElement");
        }
        ExsitingContainers[Key] = element;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if(Key is null)
        {
            throw new ArgumentNullException(nameof(Key));
        }

        if (ExsitingContainers.ContainsKey(Key))
        {
            throw new InvalidOperationException($"Key: \"{Key}\" was allready added");
        }

        if(Content is null)
        {
            Content = new ContentControl();
        }
    }
}
