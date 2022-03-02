using System;
using xFrame.Core.MVVM;

namespace xFrame.Core.ViewInjection
{
    public interface IDialogWindow
    {
        object DialogContent { get; set; }
        object DataContext { get; set; }

        event EventHandler Loaded;
        bool? ShowDialog();
        void Close();
        
    }
}