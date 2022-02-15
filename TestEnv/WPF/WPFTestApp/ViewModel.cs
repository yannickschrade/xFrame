using System;
using System.Diagnostics;
using System.Windows;
using xFrame.Core.Attributes;
using xFrame.Core.Commands;
using xFrame.Core.Fluent;
using xFrame.Core.MVVM;
using xFrame.WPF.Extensions;

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
            When(x => x.Name)
                .HasChanged()
                .Execute(p => Debug.WriteLine(p))
                .IF(v => v.CanExecute(v.Name))
                .NotifyCommand(x => x.Command);
        }
    }
}
