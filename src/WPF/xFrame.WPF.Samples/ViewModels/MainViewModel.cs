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

        [Generateproperty]
        private string _text;

        [Generateproperty]
        private RelayCommand _textCommand;


        public MainViewModel(IModuleProvider moduleProvider, ILogger<MainViewModel> logger)
        {
            TextCommand = new RelayCommand(P => MessageBox.Show("Test"));
            moduleProvider.LoadAllModules(m => logger.LogInformation("loaded Module {moduleName}", m.Name));
        }

        public override void SetupValidation()
        {


            this.Property(x => x.Text)
                .AddValidation(v =>
                {
                    v.IsNotEmpty();
                    v.NotifyCommand(x => TextCommand);
                });
        }
    }
}
