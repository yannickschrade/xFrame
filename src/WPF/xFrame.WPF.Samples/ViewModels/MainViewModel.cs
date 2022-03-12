using System.Windows;
using xFrame.Core.Attributes;
using xFrame.Core.Commands;
using xFrame.Core.Context;
using xFrame.Core.MVVM;
using xFrame.Core.Validation;
using xFrame.Modularity.Abstraction;

namespace xFrame.WPF.Samples.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {

        [Generateproperty]
        private string _text;

        [Generateproperty]
        private RelayCommand _textCommand;


        public MainViewModel(IModuleManager moduleManager)
        {
            moduleManager.InitializeModules();
            TextCommand = new RelayCommand(P => MessageBox.Show("Test"));
            
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
