using System;
using System.Collections.Generic;
using System.Text;
using xFrame.Core.MVVM;

namespace xFrame.Core.ViewInjection
{
    public interface IDialogService
    {
        void Show<T>() 
            where T : IViewModel;
        void Show<T>(T viewModel)
            where T : IViewModel;
        DialogResult ShowDialog<T>()
            where T : IViewModel;
        DialogResult ShowDialog<T>(DialogResult dialogResult)
            where T : IViewModel;
    }
}
