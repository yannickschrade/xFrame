using System.Windows;
using xFrame.WPF.Controls;

namespace xFrame.WPF.ViewInjection;

public class ViewInjector
{
    public static readonly Dictionary<object, UIElement> ExsitingContainers = new();

    public static readonly DependencyProperty ContainerKeyProperty = DependencyProperty.RegisterAttached("Key", typeof(object), typeof(ViewContainer));

    public static void SetKey(UIElement uIElement, object key)
    {
        if (ExsitingContainers.ContainsKey(key))
        {
            // TODO: CustomExcpetion
            throw new Exception();
        }
        ExsitingContainers.Add(key, uIElement);
    }
}
