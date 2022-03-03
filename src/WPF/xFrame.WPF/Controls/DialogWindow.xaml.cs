using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using xFrame.Core.ViewInjection;

namespace xFrame.WPF.Controls
{
    /// <summary>
    /// Interaktionslogik für DialogWindow.xaml
    /// </summary>
    internal partial class DialogWindow : Window, IDialogWindow
    {
        private readonly List<Button> _dialogButtons = new List<Button>();
        private double _maxWidht = 0;
        private double _maxHeight = 0;
        public DialogWindow()
        {
            InitializeComponent();
        }


        public object DialogContent
        {
            get => DialogContentPresenter.Content;
            set => DialogContentPresenter.Content = value;
        }

        event EventHandler IDialogWindow.Loaded
        {
            add => Loaded += (s, e) => value(this, null);
            remove => Loaded -= (s, e) => value(this, null);
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn.ActualWidth > _maxWidht)
            {
                _maxWidht = btn.ActualWidth;
                foreach (var button in _dialogButtons)
                {
                    button.Width = _maxWidht;
                }
            }

            if (btn.ActualHeight > _maxHeight)
            {
                _maxHeight = btn.ActualHeight;
                foreach (var button in _dialogButtons)
                {
                    button.Height = _maxHeight;
                }
            }

            btn.Width = _maxWidht;
            btn.Height = _maxHeight;
            _dialogButtons.Add(btn);


        }
    }
}
