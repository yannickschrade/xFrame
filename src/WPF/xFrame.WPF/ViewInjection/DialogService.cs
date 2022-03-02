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


        public T Show<T>() where T : IDialogViewModel
        {
            var window = CreateDialogWindow<T>(out var vm);
            window.ShowDialog();
            return vm;
        }

        public void Show<T>(T viewModel) where T : IDialogViewModel
        {
            var window = CreateDialogWindow(viewModel);
            window.ShowDialog();
        }
        
        private IDialogWindow CreateDialogWindow<T>(out T viewModel)
            where T : IDialogViewModel
        {
            var dialogWindow = _typeProvider.Resolve<IDialogWindow>();
            var content = _viewProvider.GetViewForViewModel<T>();

            var vm  = (T)content.DataContext;
            viewModel = vm;
            dialogWindow.Loaded += (s, e) => vm.OnLoaded();
            dialogWindow.DataContext = vm;
            dialogWindow.DialogContent = content;

            return dialogWindow;
        }

        private IDialogWindow CreateDialogWindow<T>(T viewModel)
            where T : IDialogViewModel
        {
            var dialogWindow = _typeProvider.Resolve<IDialogWindow>();
            var content = _viewProvider.GetViewForViewModel(viewModel);
            dialogWindow.DataContext = viewModel;
            dialogWindow.Loaded += (s, e) => viewModel.OnLoaded();
            dialogWindow.DialogContent = content;

            return dialogWindow;
        }
    }
}
