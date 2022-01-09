using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using xFrame.Core.MVVM;

namespace xFrame.WinUI3.ViewAdapters
{
    public class ItemsControlAdapter : ViewAdapterBase<ItemsControl>
    {
        public override void AddToView(ItemsControl View, FrameworkElement child)
        {
            View.Items.Add(child);
            if (child.DataContext is ViewModelBase vm)
                vm.OnViewStateChanged(true);
        }

        public override void RemoveFromView(ItemsControl View, FrameworkElement child)
        {
            View.Items.Remove(child);
            if (child.DataContext is ViewModelBase vm)
                vm.OnViewStateChanged(false);
        }

        public override void RemoveAllChildren(ItemsControl View)
        {
            foreach (var item in View.Items)
            {
                if (item is FrameworkElement element && element.DataContext is ViewModelBase vm)
                    vm.OnViewStateChanged(false);
            }

            View.Items.Clear();
        }


    }
}
