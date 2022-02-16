using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewAdapters
{
    public class SelectorAdapter : ViewAdapterBase<Selector>
    {
        private bool _first = true;

        public override void AddToView(Selector View, FrameworkElement child)
        {
            if (_first)
            {
                _first = false;
                View.SelectionChanged += SelectionChanged;
            }

            View.Items.Add(child);
        }



        public override void RemoveAllChildren(Selector View)
        {
            foreach (var item in View.Items)
            {
                if (item is FrameworkElement element && element.DataContext is IViewModel vm)
                    vm.OnViewStateChanged(false);
            }

            View.Items.Clear();
        }

        public override void RemoveFromView(Selector View, FrameworkElement child)
        {
            View.Items.Remove(child);
            if (child.DataContext is IViewModel vm)
            {
                vm.OnViewStateChanged(false);
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                if (item is FrameworkElement element && element.DataContext is IViewModel vm)
                    vm.OnViewStateChanged(true);
            }

            foreach (var item in e.RemovedItems)
            {
                if (item is FrameworkElement element && element.DataContext is IViewModel vm)
                    vm.OnViewStateChanged(false);
            }
        }
    }
}
