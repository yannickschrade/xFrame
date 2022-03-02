using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Windows.Win32;

namespace xFrame.WPF.Controls.Windows
{
    [TemplatePart(Name = PART_CloseCommand, Type = typeof(Button))]
    [TemplatePart(Name = PART_MinimizeCommand, Type = typeof(Button))]
    [TemplatePart(Name = PART_MaximizeCommand, Type = typeof(Button))]
    [TemplatePart(Name = PART_RestoreCommand, Type = typeof(Button))]
    public class WindowCommands : ItemsControl, IDisposable
    {
        private const string PART_MinimizeCommand = nameof(PART_MinimizeCommand);
        private const string PART_MaximizeCommand = nameof(PART_MaximizeCommand);
        private const string PART_RestoreCommand = nameof(PART_RestoreCommand);
        private const string PART_CloseCommand = nameof(PART_CloseCommand);

        private SafeHandle _user32;

        private Button _minimizeButton;
        private Button _maximizeButton;
        private Button _restoreButton;
        private Button _closeButton;

        static WindowCommands()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCommands), new FrameworkPropertyMetadata(typeof(WindowCommands)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _minimizeButton = Template.FindName(PART_MinimizeCommand, this) as Button;
            if (_minimizeButton is not null)
            {
                _minimizeButton.Click += MinimizeClick;
            }

            _maximizeButton = Template.FindName(PART_MaximizeCommand, this) as Button;
            if (_maximizeButton is not null)
            {
                _maximizeButton.Click += MaximiseClick;
            }

            _restoreButton = Template.FindName(PART_RestoreCommand, this) as Button;
            if (_restoreButton is not null)
            {
                _restoreButton.Click += RestoreClick;
            }

            _closeButton = GetTemplateChild(PART_CloseCommand) as Button;
            if (_closeButton is not null)
            {
                _closeButton.Click += CloseClick;
            }
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            var parentWinodw = GetParentWindow();
            if(parentWinodw is not null)
                ControlzEx.SystemCommands.CloseWindow(parentWinodw);
        }

        private void RestoreClick(object sender, RoutedEventArgs e)
        {
            var parentWindow = GetParentWindow();
            if (parentWindow is not null)
                ControlzEx.SystemCommands.RestoreWindow(parentWindow);
        }

        private void MaximiseClick(object sender, RoutedEventArgs e)
        {
            var parentWindow = GetParentWindow();
            if (parentWindow is not null)
                ControlzEx.SystemCommands.MaximizeWindow(parentWindow);
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            var parentWindow = GetParentWindow();
            if (parentWindow is not null)
                ControlzEx.SystemCommands.MinimizeWindow(parentWindow);
        }

        private unsafe string GetCaption(uint id)
        {
            if (_user32 is null)
            {
                _user32 = PInvoke.LoadLibrary(Path.Combine(Environment.SystemDirectory, "User32.dll"));
            }

            fixed (char* pchars = new char[256])
            {
                //PWSTR str = new PWSTR()
                if (PInvoke.LoadString(_user32, id, pchars, 256) == 0)
                {
                    return string.Format("String with id '{0}' could not be found.", id);
                }

                return new string(pchars).Replace("&", string.Empty);
            }
        }

        private System.Windows.Window GetParentWindow()
        {
            var window = System.Windows.Window.GetWindow(this);

            if (window is not null)
            {
                return window;
            }

            var parent = VisualTreeHelper.GetParent(this);
            System.Windows.Window parentWindow = null;

            while (parent is not null
                && (parentWindow = parent as XWindow) is null)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parentWindow;
        }


        #region IDisposable

        private bool _disposed;
        private object _minimizeCommandIcon;

        ~WindowCommands()
        {
            this.Dispose(false);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (_disposed)
            {
                return;
            }

            // If disposing equals true, dispose all managed
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
            }

            // Call the appropriate methods to clean up
            // unmanaged resources here.
            // If disposing is false,
            // only the following code is executed.
            if (_user32 is not null)
            {
                _user32.Close();
                _user32 = null;
            }

            // Note disposing has been done.
            _disposed = true;
        }

        #endregion
    }
}
