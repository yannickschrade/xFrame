using System;
using System.Windows;
using System.Windows.Controls;
using xFrame.Core.ViewService;

namespace xFrame.WPF.ViewAdapters
{
    public class PanelAdapter : ViewAdapterBase<Panel>
    {
        public override void Add(object view, Panel container)
        {
            if(view is UIElement uIElement)
            {
                container.Children.Add(uIElement);
                return;
            }

            throw new NotSupportedException();
            
        }

        public override void NavigateTo(object view, Panel container)
        {
            throw new NotSupportedException();
        }

        public override void Remove(object view, Panel container)
        {
            if (view is UIElement uIElement)
            {
                container.Children.Remove(uIElement);
                return;
            }

            throw new NotSupportedException();
        }
    }
}