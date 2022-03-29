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
using System.Windows.Navigation;
using System.Windows.Shapes;
using xFrame.WPF.Samples.ViewModels;
using xFrame.WPF.ViewInjection;

namespace xFrame.WPF.Samples.Views
{
    /// <summary>
    /// Interaktionslogik für SplashControl.xaml
    /// </summary>
    public partial class SplashControl : UserControl, IViewFor<SplashViewModel>
    {
        public SplashControl()
        {
            InitializeComponent();
        }
    }
}
