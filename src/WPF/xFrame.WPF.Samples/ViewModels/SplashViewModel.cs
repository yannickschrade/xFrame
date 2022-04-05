using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xFrame.Core.Generators;
using xFrame.Core.MVVM;
using xFrame.WPF.Theming.Templates;

namespace xFrame.WPF.Samples.ViewModels
{

    internal partial class SplashViewModel : ViewModelBase, ISplashScreenViewModel
    {
        private readonly ILogger _logger;

        [Generateproperty]
        private string _message = "Test";

        public string StyleKey => "SplashWindowStyle";

        public SplashViewModel(ILogger<SplashViewModel> logger)
        {
            _logger = logger;
        }

        public async Task LoadAppAsync()
        {
            Message = "Hello From Splash Screen";
            await Task.Delay(5000);
            Message = "Little Progress update";
            await Task.Delay(5000);
            Message = "Allready done";
            await Task.Delay(2000);
            _logger.LogInformation("Hello World");
        }

    }
}
