using System.Windows;
using System.Windows.Controls;

namespace xFrame.WPF.ViewAdapters
{
    internal class ContentControlAdapter : ViewAdapterBase<ContentControl>
    {
        public override void AddToView(ContentControl View, FrameworkElement child)
        {
            View.Content = child;
        }

        public override void RemoveAllChildren(ContentControl View)
        {
            View.Content = null;
        }

        public override void RemoveFromView(ContentControl View, FrameworkElement child)
        {
            View.Content = null;
        }
    }
}
