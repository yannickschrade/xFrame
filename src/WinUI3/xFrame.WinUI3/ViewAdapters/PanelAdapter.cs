using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using xFrame.Core.MVVM;

namespace xFrame.WinUI3.ViewAdapters
{
    internal class PanelAdapter : ViewAdapterBase<Panel>
    {
        public override void AddToView(Panel View, FrameworkElement child)
        {
            View.Children.Add(child);
            if (child.DataContext is ViewModelBase vm)
            {
                vm.OnViewStateChanged(true);
            }
        }

        public override void RemoveAllChildren(Panel View)
        {
            foreach (var child in View.Children)
            {
                if (child is FrameworkElement element && element.DataContext is ViewModelBase vm)
                {
                    vm.OnViewStateChanged(false);
                }
            }
            View.Children.Clear();
        }

        public override void RemoveFromView(Panel View, FrameworkElement child)
        {
            View.Children.Remove(child);
            if (child.DataContext is ViewModelBase vm)
            {
                vm.OnViewStateChanged(false);
            }
        }
    }
}
