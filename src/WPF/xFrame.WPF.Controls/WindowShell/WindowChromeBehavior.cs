//Code from controlzEx library https://github.com/ControlzEx/ControlzEx
using System;
using System.Collections.Generic;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using Microsoft.Xaml.Behaviors;
using xFrame.WPF.Controls.Native;
using xFrame.WPF.Controls.Helper;
using xFrame.WPF.Controls.Internal;

namespace xFrame.WPF.Controls.WindowShell
{
    public enum WindowCornerPreference
    {
        /// <summary>
        /// Use the windows default.
        /// </summary>
        Default = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_DEFAULT,

        /// <summary>
        /// Do NOT round window corners.
        /// </summary>
        DoNotRound = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_DONOTROUND,

        /// <summary>
        /// Round window corners.
        /// </summary>
        Round = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND,

        /// <summary>
        /// Round window corners with small radius.
        /// </summary>
        RoundSmall = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUNDSMALL,
    }

    /// <summary>
    /// With this class we can make custom window styles.
    /// </summary>
    public partial class WindowChromeBehavior : Behavior<Window>
    {
        /// <summary>Underlying HWND for the _window.</summary>
        /// <SecurityNote>
        ///   Critical : Critical member
        /// </SecurityNote>
        [SecurityCritical]
        private HWND windowHandle;

        /// <summary>Underlying HWND for the _window.</summary>
        /// <SecurityNote>
        ///   Critical : Critical member provides access to HWND's window messages which are critical
        /// </SecurityNote>
        [SecurityCritical]
        private HwndSource hwndSource;

        private PropertyChangeNotifier borderThicknessChangeNotifier;
        private Thickness? savedBorderThickness;

        private bool isCleanedUp;

        private readonly Thickness cornerGripThickness = new(Constants.ResizeCornerGripThickness);

        private struct SystemParameterBoundProperty
        {
            public string SystemParameterPropertyName { get; set; }

            public DependencyProperty DependencyProperty { get; set; }
        }

        /// <summary>
        /// Mirror property for <see cref="ResizeBorderThickness"/>.
        /// </summary>
        public Thickness ResizeBorderThickness
        {
            get { return (Thickness)GetValue(ResizeBorderThicknessProperty); }
            set { SetValue(ResizeBorderThicknessProperty, value); }
        }

        /// <summary>Identifies the <see cref="ResizeBorderThickness"/> dependency property.</summary>
        public static readonly DependencyProperty ResizeBorderThicknessProperty =
            DependencyProperty.Register(nameof(ResizeBorderThickness), typeof(Thickness), 
                typeof(WindowChromeBehavior), 
                new PropertyMetadata(GetDefaultResizeBorderThickness(), OnResizeBorderThicknessChanged), 
                (value) => ((Thickness)value).IsNonNegative());

        private static void OnResizeBorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (WindowChromeBehavior)d;
            behavior._OnChromePropertyChangedThatRequiresRepaint();
        }

        /// <summary>
        /// Defines if the Taskbar should be ignored when maximizing a Window.
        /// This only works with WindowStyle = None.
        /// </summary>
        public bool IgnoreTaskbarOnMaximize
        {
            get { return (bool)GetValue(IgnoreTaskbarOnMaximizeProperty); }
            set { SetValue(IgnoreTaskbarOnMaximizeProperty, value); }
        }

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="IgnoreTaskbarOnMaximize"/>.
        /// </summary>
        public static readonly DependencyProperty IgnoreTaskbarOnMaximizeProperty =
            DependencyProperty.Register(nameof(IgnoreTaskbarOnMaximize), typeof(bool), typeof(WindowChromeBehavior), new PropertyMetadata(false, OnIgnoreTaskbarOnMaximizeChanged));

        /// <summary>
        /// Gets/sets if the border thickness value should be kept on maximize
        /// if the MaxHeight/MaxWidth of the window is less than the monitor resolution.
        /// </summary>
        public bool KeepBorderOnMaximize
        {
            get { return (bool)GetValue(KeepBorderOnMaximizeProperty); }
            set { SetValue(KeepBorderOnMaximizeProperty, value); }
        }

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="KeepBorderOnMaximize"/>.
        /// </summary>
        public static readonly DependencyProperty KeepBorderOnMaximizeProperty = DependencyProperty.Register(nameof(KeepBorderOnMaximize), typeof(bool), typeof(WindowChromeBehavior), new PropertyMetadata(true, OnKeepBorderOnMaximizeChanged));

        private static readonly DependencyPropertyKey IsNCActivePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsNCActive), typeof(bool), typeof(WindowChromeBehavior), new PropertyMetadata(false));

        /// <summary>
        /// <see cref="DependencyProperty"/> for <see cref="IsNCActive"/>.
        /// </summary>
        public static readonly DependencyProperty IsNCActiveProperty = IsNCActivePropertyKey.DependencyProperty;

        /// <summary>
        /// Gets whether the non-client area is active or not.
        /// </summary>
        public bool IsNCActive
        {
            get { return (bool)GetValue(IsNCActiveProperty); }
            private set { SetValue(IsNCActivePropertyKey, value); }
        }

        public static readonly DependencyProperty EnableMinimizeProperty = DependencyProperty.Register(nameof(EnableMinimize), typeof(bool), typeof(WindowChromeBehavior), new PropertyMetadata(true, OnEnableMinimizeChanged));

        private static void OnEnableMinimizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue && e.NewValue is bool showMinButton)
            {
                var behavior = (WindowChromeBehavior)d;

                behavior.UpdateMinimizeSystemMenu(showMinButton);
            }
        }

        private void UpdateMinimizeSystemMenu(bool isVisible)
        {
            if (windowHandle == IntPtr.Zero)
            {
                return;
            }

            if (hwndSource?.IsDisposed == true)
            {
                return;
            }

            if (isVisible)
            {
                this._ModifyStyle(0, WINDOW_STYLE.WS_MINIMIZEBOX);
            }
            else
            {
                this._ModifyStyle(WINDOW_STYLE.WS_MINIMIZEBOX, 0);
            }

            this._UpdateSystemMenu(AssociatedObject?.WindowState);
        }

        /// <summary>
        /// Gets or sets whether if the minimize button is visible and the minimize system menu is enabled.
        /// </summary>
        public bool EnableMinimize
        {
            get { return (bool)GetValue(EnableMinimizeProperty); }
            set { this.SetValue(EnableMinimizeProperty, value); }
        }

        public static readonly DependencyProperty EnableMaxRestoreProperty = DependencyProperty.Register(nameof(EnableMaxRestore), typeof(bool), typeof(WindowChromeBehavior), new PropertyMetadata(true, OnEnableMaxRestoreChanged));

        private static void OnEnableMaxRestoreChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue && e.NewValue is bool showMaxRestoreButton)
            {
                var behavior = (WindowChromeBehavior)d;

                behavior.UpdateMaxRestoreSystemMenu(showMaxRestoreButton);
            }
        }

        private void UpdateMaxRestoreSystemMenu(bool isVisible)
        {
            if (windowHandle == IntPtr.Zero)
            {
                return;
            }

            if (hwndSource?.IsDisposed == true)
            {
                return;
            }

            if (isVisible)
            {
                this._ModifyStyle(0, WINDOW_STYLE.WS_MAXIMIZEBOX);
            }
            else
            {
                this._ModifyStyle(WINDOW_STYLE.WS_MAXIMIZEBOX, 0);
            }

            this._UpdateSystemMenu(AssociatedObject?.WindowState);
        }

        /// <summary>
        /// Gets or sets whether if the maximize/restore button is visible and the maximize/restore system menu is enabled.
        /// </summary>
        public bool EnableMaxRestore
        {
            get { return (bool)GetValue(EnableMaxRestoreProperty); }
            set { this.SetValue(EnableMaxRestoreProperty, value); }
        }

        public static readonly DependencyProperty CornerPreferenceProperty =
            DependencyProperty.Register(nameof(CornerPreference), typeof(WindowCornerPreference), typeof(WindowChromeBehavior), new PropertyMetadata(WindowCornerPreference.Default, OnCornerPreferenceChanged));

        public WindowCornerPreference CornerPreference
        {
            get => (WindowCornerPreference)GetValue(CornerPreferenceProperty);
            set => SetValue(CornerPreferenceProperty, value);
        }

        private static void OnCornerPreferenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (WindowChromeBehavior)d;

            behavior.UpdateDWMCornerPreference((WindowCornerPreference)e.NewValue);
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();

            // no transparency, because it has more then one unwanted issues
            if (AssociatedObject.AllowsTransparency)
            {
                try
                {
                    AssociatedObject.SetCurrentValue(Window.AllowsTransparencyProperty, false);
                }
                catch (Exception)
                {
                    //For some reason, we can't determine if the window has loaded or not, so we swallow the exception.
                }
            }

            savedBorderThickness = AssociatedObject.BorderThickness;
            borderThicknessChangeNotifier = new PropertyChangeNotifier(AssociatedObject, Control.BorderThicknessProperty);
            borderThicknessChangeNotifier.ValueChanged += BorderThicknessChangeNotifierOnValueChanged;

            AssociatedObject.Closed += AssociatedObject_Closed;
            AssociatedObject.StateChanged += AssociatedObject_StateChanged;

            Initialize();
        }

        /// <summary>
        /// Gets the default resize border thickness from the system parameters.
        /// </summary>
        public static Thickness GetDefaultResizeBorderThickness()
        {
            var dpiX = PInvoke.GetDeviceCaps(PInvoke.GetDC(default), GET_DEVICE_CAPS_INDEX.LOGPIXELSX);
            var dpiY = PInvoke.GetDeviceCaps(PInvoke.GetDC(default), GET_DEVICE_CAPS_INDEX.LOGPIXELSY);
            var xframe = PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXFRAME);
            var yframe = PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CYFRAME);
            var padding = PInvoke.GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXPADDEDBORDER);
            xframe += padding;
            yframe += padding;
            var logical = DpiHelper.DeviceSizeToLogical(new Size(xframe, yframe), dpiX / 96.0, dpiY / 96.0);
            return new Thickness(logical.Width, logical.Height, logical.Width, logical.Height);
        }

        private void BorderThicknessChangeNotifierOnValueChanged(object sender, EventArgs e)
        {
            // It's bad if the window is null at this point, but we check this here to prevent the possible occurred exception
            var window = AssociatedObject;
            if (window is not null)
            {
                savedBorderThickness = window.BorderThickness;
            }
        }

        private static void OnIgnoreTaskbarOnMaximizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (WindowChromeBehavior)d;

            // A few things to consider when removing the below hack
            // - ResizeMode="NoResize"
            //   WindowState="Maximized"
            //   IgnoreTaskbarOnMaximize="True"
            // - Changing IgnoreTaskbarOnMaximize while window is maximized

            // Changing the WindowState solves all, known, issues with changing IgnoreTaskbarOnMaximize.
            // Since IgnoreTaskbarOnMaximize is not changed all the time this hack seems to be less risky than anything else.
            if (behavior.AssociatedObject?.WindowState == WindowState.Maximized)
            {
                behavior._OnChromePropertyChangedThatRequiresRepaint();

                behavior.AssociatedObject.SetCurrentValue(Window.WindowStateProperty, WindowState.Normal);
                behavior.AssociatedObject.SetCurrentValue(Window.WindowStateProperty, WindowState.Maximized);
            }
        }

        private static void OnKeepBorderOnMaximizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (WindowChromeBehavior)d;

            behavior.HandleStateChanged();
        }

        [SecuritySafeCritical]
        private void Cleanup(bool isClosing)
        {
            if (isCleanedUp)
            {
                return;
            }

            isCleanedUp = true;

            OnCleanup();

            // clean up events
            AssociatedObject.Closed -= AssociatedObject_Closed;
            AssociatedObject.StateChanged -= AssociatedObject_StateChanged;

            hwndSource?.RemoveHook(this.WindowProc);
        }

        /// <summary>
        /// Occurs during the cleanup of this behavior.
        /// </summary>
        protected virtual void OnCleanup()
        {
            // nothing here
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            Cleanup(false);

            base.OnDetaching();
        }

        private void Initialize()
        {
            windowHandle = new(new WindowInteropHelper(AssociatedObject).EnsureHandle());
            this.nonClientControlManager = new NonClientControlManager(AssociatedObject);

            if (windowHandle == IntPtr.Zero)
            {
                throw new Exception("Uups, at this point we really need the Handle from the associated object!");
            }

            if (AssociatedObject.SizeToContent != SizeToContent.Manual
                && AssociatedObject.WindowState == WindowState.Normal)
            {
                // Another try to fix SizeToContent
                // without this we get nasty glitches at the borders
                Invoke(AssociatedObject, () =>
                                              {
                                                  AssociatedObject.InvalidateMeasure();
                                              });
            }

            hwndSource = HwndSource.FromHwnd(windowHandle);
            hwndSource?.AddHook(this.WindowProc);

            this._ApplyNewCustomChrome();

            // handle the maximized state here too (to handle the border in a correct way)
            HandleStateChanged();
        }

        private void AssociatedObject_Closed(object sender, EventArgs e)
        {
            Cleanup(true);
        }

        private void AssociatedObject_StateChanged(object sender, EventArgs e)
        {
            HandleStateChanged();
        }

        private void HandleStateChanged()
        {
            HandleBorderThicknessDuringMaximize();

            if (AssociatedObject.WindowState == WindowState.Maximized)
            {
                // Workaround for:
                // MaxWidth="someValue"
                // SizeToContent = "WidthAndHeight"
                // Dragging the window to the top with those things set does not change the height of the Window
                if (AssociatedObject.SizeToContent != SizeToContent.Manual)
                {
                    AssociatedObject.SetCurrentValue(Window.SizeToContentProperty, SizeToContent.Manual);
                }
            }
            else if (AssociatedObject.WindowState == WindowState.Normal
                     && IgnoreTaskbarOnMaximize)
            {
                // Required to fix wrong NC area rendering.
                ForceNativeWindowRedraw();
            }
        }

        private void ForceNativeWindowRedraw()
        {
            if (windowHandle == IntPtr.Zero
                || hwndSource is null
                || hwndSource.IsDisposed)
            {
                return;
            }

            PInvoke.SetWindowPos(windowHandle, default, 0, 0, 0, 0, SwpFlags);
        }

        /// <summary>
        /// This fix is needed because style triggers don't work if someone sets the value locally/directly on the window.
        /// </summary>
        private void HandleBorderThicknessDuringMaximize()
        {
            borderThicknessChangeNotifier!.RaiseValueChanged = false;

            if (AssociatedObject.WindowState == WindowState.Maximized)
            {
                var monitor = IntPtr.Zero;

                if (windowHandle != IntPtr.Zero)
                {
                    monitor = PInvoke.MonitorFromWindow(windowHandle, MONITOR_FROM_FLAGS.MONITOR_DEFAULTTONEAREST);
                }

                if (monitor != IntPtr.Zero)
                {
                    var rightBorderThickness = 0D;
                    var bottomBorderThickness = 0D;

                    if (KeepBorderOnMaximize
                        && savedBorderThickness.HasValue)
                    {
                        var monitorInfo = PInvoke.GetMonitorInfo(monitor);
                        var monitorRect = IgnoreTaskbarOnMaximize ? monitorInfo.rcMonitor : monitorInfo.rcWork;

                        // If the maximized window will have a width less than the monitor size, show the right border.
                        if (AssociatedObject.MaxWidth < monitorRect.GetWidth())
                        {
                            rightBorderThickness = savedBorderThickness.Value.Right;
                        }

                        // If the maximized window will have a height less than the monitor size, show the bottom border.
                        if (AssociatedObject.MaxHeight < monitorRect.GetHeight())
                        {
                            bottomBorderThickness = savedBorderThickness.Value.Bottom;
                        }
                    }

                    // set window border, so we can move the window from top monitor position
                    AssociatedObject.SetCurrentValue(Control.BorderThicknessProperty, new Thickness(0, 0, rightBorderThickness, bottomBorderThickness));
                }
                else // Can't get monitor info, so just remove all border thickness
                {
                    AssociatedObject.SetCurrentValue(Control.BorderThicknessProperty, new Thickness(0));
                }
            }
            else
            {
                AssociatedObject.SetCurrentValue(Control.BorderThicknessProperty, savedBorderThickness.GetValueOrDefault(new Thickness(0)));
            }

            borderThicknessChangeNotifier.RaiseValueChanged = true;
        }

        private bool UpdateDWMCornerPreference(WindowCornerPreference cornerPreference)
        {
            return UpdateDWMCornerPreference((DWM_WINDOW_CORNER_PREFERENCE)cornerPreference);
        }

        private bool UpdateDWMCornerPreference(DWM_WINDOW_CORNER_PREFERENCE cornerPreference)
        {
            if (windowHandle == IntPtr.Zero)
            {
                return false;
            }

            return DwmHelper.SetWindowAttributeValue(windowHandle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, (int)cornerPreference);
        }

        private static void Invoke(DispatcherObject dispatcherObject, Action invokeAction)
        {
            if (dispatcherObject is null)
            {
                throw new ArgumentNullException(nameof(dispatcherObject));
            }

            if (invokeAction is null)
            {
                throw new ArgumentNullException(nameof(invokeAction));
            }

            if (dispatcherObject.Dispatcher.CheckAccess())
            {
                invokeAction();
            }
            else
            {
                dispatcherObject.Dispatcher.Invoke(invokeAction);
            }
        }

        private static readonly List<SystemParameterBoundProperty> boundProperties = new()
        {
            new SystemParameterBoundProperty { DependencyProperty = ResizeBorderThicknessProperty, SystemParameterPropertyName = nameof(SystemParameters.WindowResizeBorderThickness) },
        };
    }
}