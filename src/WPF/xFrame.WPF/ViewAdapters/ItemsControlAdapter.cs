﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using xFrame.Core.MVVM;

namespace xFrame.WPF.ViewAdapters
{
    public class ItemsControlAdapter : ViewAdapterBase<ItemsControl>
    {
        public override void AddToView(ItemsControl View, FrameworkElement child)
        {
            View.Items.Add(child);
            if (child.DataContext is IViewModel vm)
                vm.OnViewStateChanged(true);
        }

        public override void RemoveFromView(ItemsControl View, FrameworkElement child)
        {
            View.Items.Remove(child);
            if (child.DataContext is IViewModel vm)
                vm.OnViewStateChanged(false);
        }

        public override void RemoveAllChildren(ItemsControl View)
        {
            foreach (var item in View.Items)
            {
                if (item is FrameworkElement element && element.DataContext is IViewModel vm)
                    vm.OnViewStateChanged(false);
            }

            View.Items.Clear();
        }

        
    }
}
