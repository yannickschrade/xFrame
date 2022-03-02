using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using xFrame.Core.ViewInjection;

namespace xFrame.WPF.Controls.Windows
{
    /// <summary>
    /// Interaktionslogik für DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : XWindow, IDialogWindow
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
