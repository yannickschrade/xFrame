using System;
using System.Windows;
using System.Windows.Controls;
using xFrame.Core.ViewService;

namespace xFrame.WPF.ViewAdapters
{
    public class ItemsControlAdapter : ViewAdapterBase<ItemsControl>
    {
        public override void Add(object view, ItemsControl container)
        {
            container.Items.Add(view);
        }

        public override void NavigateTo(object view, ItemsControl container)
        {
            throw new NotSupportedException();
        }

        public override void Remove(object view, ItemsControl container)
        {
            container.Items.Remove(view);
        }
    }
}