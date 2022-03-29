using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xFrame.Core.MVVM;

namespace xFrame.WPF
{
    public interface ISplashScreenViewModel : IViewModel
    {
        string StyleKey { get; }
        Task LoadAppAsync();
    }
}
