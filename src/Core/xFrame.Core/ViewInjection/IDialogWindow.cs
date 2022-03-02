using System;
using xFrame.Core.MVVM;

namespace xFrame.Core.ViewInjection
{
    public interface IDialogWindow
    {
        void Show();
        void ShowDialog();
        object DialogContent { get; set; }

        object DataContext { get; set; }

        event EventHandler Loaded;
    }
}