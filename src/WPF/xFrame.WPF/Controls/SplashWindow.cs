using System.Windows;
using xFrame.Core.MVVM;
using xFrame.WPF.ViewService;

namespace xFrame.WPF.Controls;

public class SplashWindow<T> : SplashWindow, IViewFor<T>
    where T : ViewModelBase, ISplashViewModel
{
    new public T? DataContext { get; set; }
}

public abstract class SplashWindow : Window
{
    public new ISplashViewModel? DataContext { get; set; }

    protected SplashWindow()
    {

    }
}