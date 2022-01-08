using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace xFrame.WPF.ViewAdapters
{
    internal class PanelAdapter : ViewAdapterBase<Panel>
    {
        public override void AddToView(Panel View, FrameworkElement child)
        {
            View.Children.Add(child);
        }

        public override void RemoveAllChildren(Panel View)
        {
            View.Children.Clear();
        }

        public override void RemoveFromView(Panel View, FrameworkElement child)
        {
            View.Children.Remove(child);
        }
    }
}
