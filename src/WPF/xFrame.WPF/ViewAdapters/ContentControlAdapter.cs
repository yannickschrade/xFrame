using System.Windows;
using System.Windows.Controls;
using xFrame.Core.ViewService;

namespace xFrame.WPF.ViewAdapters
{
    public class ContentControlAdapter : ViewAdapterBase<ContentControl>
    {
        public override void Add(object view, ContentControl container)
        {
            container.Content = view;
        }

        public override void NavigateTo(object view, ContentControl container)
        {
            container.Content = view;
        }

        public override void Remove(object view, ContentControl container)
        {
            container.Content = null;
        }
    }
}