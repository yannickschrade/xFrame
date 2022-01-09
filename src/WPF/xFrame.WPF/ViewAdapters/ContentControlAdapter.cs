using System.Windows;
using System.Windows.Controls;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewAdapters
{
    internal class ContentControlAdapter : ViewAdapterBase<ContentControl>
    {
        public override void AddToView(ContentControl View, FrameworkElement child)
        {
            if(View.Content != null && View.Content is FrameworkElement element && element.DataContext is ViewModelBase vmOld)
            {
                vmOld.OnViewStateChanged(false);
            }

            View.Content = child;
            
            if(child.DataContext is ViewModelBase vm)
            {
                vm.OnViewStateChanged(true);
            }
        }

        public override void RemoveAllChildren(ContentControl View)
        {
            if (View.Content != null && View.Content is FrameworkElement element && element.DataContext is ViewModelBase vmOld)
            {
                vmOld.OnViewStateChanged(false);
            }
            View.Content = null;
        }

        public override void RemoveFromView(ContentControl View, FrameworkElement child)
        {
            if (View.Content != null && View.Content is FrameworkElement element && element.DataContext is ViewModelBase vmOld)
            {
                vmOld.OnViewStateChanged(false);
            }
            View.Content = null;
        }
    }
}
