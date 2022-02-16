using System.Diagnostics;
using System.Windows;
using xFrame.Core.Attributes;
using xFrame.Core.Commands;
using xFrame.Core.Fluent;
using xFrame.Core.MVVM;

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
            Command = new RelayCommand<string>(CanExecute, p => MessageBox.Show("Works"));
            Property(x => x.Name)
                .HasChanged(x =>
                {
                    x.Execute(p => Debug.WriteLine(p));
                });
        }
    }
}
