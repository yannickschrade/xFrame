using Microsoft.Extensions.Logging;
using System.Windows;
using xFrame.Core.Attributes;
using xFrame.Core.Commands;
using xFrame.Core.Context;
using xFrame.Core.Modularity;
using xFrame.Core.MVVM;
using xFrame.Core.Validation;

namespace xFrame.WPF.Samples.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IModuleProvider _moduleProvider;

        [Generateproperty]
        private string _loadingText;

        public MainViewModel(IModuleProvider moduleProvider, ILogger<MainViewModel> logger)
        {
            LoadingText = "Lade App";
            _moduleProvider = moduleProvider;
        }

        public override void OnLoaded()
        {
            _moduleProvider.LoadAllModules(m => LoadingText = $"Module: {m.Name} geladen");

        }

        public override void SetupValidation()
        {

        }
    }
}
