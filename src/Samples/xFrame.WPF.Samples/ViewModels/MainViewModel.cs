using Microsoft.Extensions.Logging;
using xFrame.Core.Commands;
using xFrame.Core.Context;
using xFrame.Core.Generators;
using xFrame.Core.Modularity;
using xFrame.Core.MVVM;
using xFrame.Core.Validation;

namespace xFrame.WPF.Samples.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IModuleProvider _moduleProvider;

        [Generateproperty]
        private string _loadingText = "Test";

        [Generateproperty]
        private string _inputTextBox;

        private AsyncRelayCommand _backgroundCommand;
        public AsyncRelayCommand BackgroundCommand => _backgroundCommand ?? new AsyncRelayCommand(AsyncWork);


        public MainViewModel(IModuleProvider moduleProvider, ILogger<MainViewModel> logger)
        {
            LoadingText = "Lade App";
            _moduleProvider = moduleProvider;
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            _moduleProvider.LoadAllModules(m => LoadingText = $"Module: {m.Name} geladen");

        }

        public override void SetupValidation()
        {
            this.Property(x => x.InputTextBox)
                    .AddValidation(x =>
                    {
                        x.IsNotEmpty()
                        .WithMessage("Textfeld darf nicht Leer sein");
                    });
        }

        private async Task AsyncWork(CancellationToken token)
        {
            LoadingText = "Do Some Async Suff";
            await Task.Delay(5000, token);
            LoadingText = "Async Work done";
        }
    }
}
