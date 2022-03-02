using System;
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
            vm.CloseDialogAction = new Action(() => dialogWindow.Close());
            dialogWindow.DataContext = vm;
            dialogWindow.DialogContent = content;

            return dialogWindow;
        }

        private IDialogWindow CreateDialogWindow<T>(T vm)
            where T : IDialogViewModel
        {
            var dialogWindow = _typeProvider.Resolve<IDialogWindow>();
            var content = _viewProvider.GetViewForViewModel(vm);
            dialogWindow.DataContext = vm;
            vm.CloseDialogAction = new Action(() => dialogWindow.Close());
            dialogWindow.Loaded += (s, e) => vm.OnLoaded();
            dialogWindow.DialogContent = content;

            return dialogWindow;
        }
    }
}
