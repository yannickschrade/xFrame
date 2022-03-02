using System;
using System.Windows;
using xFrame.Core.ViewInjection;

namespace xFrame.WPF.Controls
{
    /// <summary>
    /// Interaktionslogik für DialogWindow.xaml
    /// </summary>
    internal partial class DialogWindow : Window, IDialogWindow
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        public object DialogContent
        {
            get => DialogContentPresenter.Content;
            set => DialogContentPresenter.Content = value;
        }

        event EventHandler IDialogWindow.Loaded
        {
            add => Loaded += (s, e) => value(this, null);
            remove => Loaded -= (s, e) => value(this, null);
        }
    }
}
