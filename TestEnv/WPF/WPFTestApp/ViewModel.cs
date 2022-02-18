using System.Diagnostics;
using System.Windows;
using xFrame.Core.Attributes;
using xFrame.Core.Commands;
using xFrame.Core.Context;
using xFrame.Core.MVVM;
using xFrame.Core.Validation;

namespace WPFTestApp
{
    public partial class ViewModel : ViewModelBase<ViewModel>
    {
        [Generateproperty]
        private string _name = "Max Mustermann";

        [Generateproperty]
        private RelayCommand<string> _command;

        private bool CanExecute(string arg)
        {
            return Name == "Test";
        }

        public ViewModel()
        {
            Command = new RelayCommand<string>(p => MessageBox.Show(Name));
            Property(x => x.Name)
                .WhenChanged(x =>
                {
                    x.Execute(p => Debug.WriteLine(p));
                })
                .AddValidation(x =>
                {
                    x.IsNotEmpty();
                    x.UpdateCommandCanExecute(x => x.Command);
                });
        }
    }
}
