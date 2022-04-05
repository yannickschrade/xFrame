using Microsoft.Extensions.Logging;
using xFrame.Core.Commands;
using xFrame.Core.Generators;
using xFrame.Core.Modularity;
using xFrame.Core.MVVM;

namespace xFrame.WPF.Samples.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IModuleProvider _moduleProvider;

        [Generateproperty]
        private string _loadingText = "Test";

        private AsyncRelayCommand _backgroundCommand;

        public AsyncRelayCommand BackgroundCommand => _backgroundCommand ?? new AsyncRelayCommand(AsyncWork);
        

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

        private async Task AsyncWork(CancellationToken token)
        {
            LoadingText = "Do Some Async Suff";
            await Task.Delay(5000, token);
            LoadingText = "Async Work done";
        }
    }
}
