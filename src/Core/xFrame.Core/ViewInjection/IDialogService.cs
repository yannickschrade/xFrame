using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.MVVM;

namespace xFrame.Core.ViewInjection
{
    public interface IDialogService
    {
        T Show<T>() 
            where T : IDialogViewModel;
        void Show<T>(T viewModel)
            where T : IDialogViewModel;
    }
}
