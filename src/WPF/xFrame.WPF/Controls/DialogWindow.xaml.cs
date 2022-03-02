using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

        void IDialogWindow.ShowDialog()
        {
            ShowDialog();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CancleButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
