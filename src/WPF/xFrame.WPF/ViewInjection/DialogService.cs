using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using xFrame.Core.IoC;
using xFrame.Core.MVVM;
using xFrame.Core.ViewInjection;

namespace xFrame.WPF.ViewInjection
{
    internal class DialogService : IDialogService
    {
        private readonly IViewProvider _viewProvider;
        private readonly ITypeProviderService _typeProvider;

        public DialogService(IViewProvider viewProvider, ITypeProviderService typeProvider)
        {
            _viewProvider = viewProvider;
            _typeProvider = typeProvider;
        }


        public void Show<T>() where T : IViewModel
        {
            var window = CreateDialogWindow<T>();
            window.Show();
        }

        public void Show<T>(T viewModel) where T : IViewModel
        {
            var window = CreateDialogWindow<T>();
            window.Show();
        }

        public DialogResult ShowDialog<T>() where T : IViewModel
        {
            var window = CreateDialogWindow<T>();
            window.ShowDialog();
        }

        public DialogResult ShowDialog<T>(DialogResult dialogResult) where T : IViewModel
        {
            var window = CreateDialogWindow<T>();
            window.ShowDialog();
        }
        
        private IDialogWindow CreateDialogWindow<T>()
            where T : IViewModel
        {
            var dialogWindow = _typeProvider.Resolve<IDialogWindow>();
            var content = _viewProvider.GetViewForViewModel<T>();
            dialogWindow.Content = content;
            return dialogWindow;
        }
    }
}
