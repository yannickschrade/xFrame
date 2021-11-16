using System;
using System.Windows;
using xFrame.Core.MVVM;
using xFrame.Core.ViewService;

namespace xFrame.WPF.Controls
{
    public class SplashWindow<T> : SplashWindow, IViewFor<T>
        where T : ViewModelBase, ISplashViewModel
    {
    }

    public abstract class SplashWindow : Window
    {
        public new ISplashViewModel? DataContext { get; set; }

        protected SplashWindow()
        {

        }
    }
}