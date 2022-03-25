using System.Windows;
using xFrame.WPF.Samples.ViewModels;
using xFrame.WPF.ViewInjection;

namespace xFrame.WPF.Samples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IViewFor<MainViewModel>, IShell
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
