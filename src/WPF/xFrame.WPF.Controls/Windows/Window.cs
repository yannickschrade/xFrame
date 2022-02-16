using ControlzEx;
using ControlzEx.Behaviors;
using ControlzEx.Native;
using Microsoft.Xaml.Behaviors;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using System.Runtime.CompilerServices;

namespace xFrame.WPF.Controls.Windows
{
    public class Window : WindowChromeWindow
    {
        private const string PART_ContentPresenter = "PART_ContentPresenter";
        private const string PART_Header = "PART_Header";
        private const string PART_WindowCommands = "PART_WindowCommands";

        public UIElement WindowCommands
        {
            get { return (UIElement)GetValue(WindowCommandsProperty); }
            set { SetValue(WindowCommandsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WindowCommands.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowCommandsProperty =
            DependencyProperty.Register(nameof(WindowCommands), typeof(UIElement), typeof(Window), new FrameworkPropertyMetadata(null));



        public UIElement WindowHeader
        {
            get { return (UIElement)GetValue(WindowHeaderProperty); }
            set { SetValue(WindowHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WindowHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowHeaderProperty =
            DependencyProperty.Register("WindowHeader", typeof(UIElement), typeof(Window), new PropertyMetadata(null));

        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
        }

        #region Behaviors

        /// <summary>
        /// Initializes the WindowChromeBehavior which is needed to render the custom WindowChrome.
        /// </summary>
        private void InitializeWindowChromeBehavior()
        {
            var behavior = new WindowChromeBehavior();
            BindingOperations.SetBinding(behavior, WindowChromeBehavior.ResizeBorderThicknessProperty, new Binding { Path = new PropertyPath(ResizeBorderThicknessProperty), Source = this });
            BindingOperations.SetBinding(behavior, WindowChromeBehavior.IgnoreTaskbarOnMaximizeProperty, new Binding { Path = new PropertyPath(IgnoreTaskbarOnMaximizeProperty), Source = this });
            BindingOperations.SetBinding(behavior, GlowWindowBehavior.GlowColorProperty, new Binding { Path = new PropertyPath(GlowColorProperty), Source = this });

            Interaction.GetBehaviors(this).Add(behavior);
        }

        /// <summary>
        /// Initializes the GlowWindowBehavior which is needed to render the custom resize windows around the current window.
        /// </summary>
        private void InitializeGlowWindowBehavior()
        {
            var behavior = new GlowWindowBehavior();
            BindingOperations.SetBinding(behavior, GlowWindowBehavior.GlowDepthProperty, new Binding { Path = new PropertyPath(GlowDepthProperty), Source = this });
            BindingOperations.SetBinding(behavior, GlowWindowBehavior.GlowColorProperty, new Binding { Path = new PropertyPath(GlowColorProperty), Source = this });
            BindingOperations.SetBinding(behavior, GlowWindowBehavior.NonActiveGlowColorProperty, new Binding { Path = new PropertyPath(NonActiveGlowColorProperty), Source = this });

            Interaction.GetBehaviors(this).Add(behavior);
        }

        #endregion

        public Window()
        {
            InitializeWindowChromeBehavior();
            InitializeGlowWindowBehavior();
            if(Icon == null)
                Icon = GetDefaultIcon(new Size(24,24));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var windowCommands = GetTemplateChild(PART_WindowCommands) as ContentPresenter;
            if (windowCommands.Content is null)
                windowCommands.Content = new WindowCommands();
           
            var headerBorder = GetTemplateChild("HeaderBorder") as Border;
            headerBorder.MouseLeftButtonDown += HeaderBorder_MouseLeftButtonDown;
            windowCommands.SetCurrentValue(WindowChrome.IsHitTestVisibleInChromeProperty, true);
        }

        private void HeaderBorder_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(e.ClickCount == 1)
            {
                e.Handled = true;
                VerifyAccess();

                PInvoke.ReleaseCapture();
                var criticalHandle = (IntPtr)(criticalHandlePropertyInfo?.GetValue(this, emptyObjectArray) ?? IntPtr.Zero);

                if (criticalHandle != IntPtr.Zero)
                {
                    // these lines are from DragMove
                    // NativeMethods.SendMessage(criticalHandle, WM.SYSCOMMAND, (IntPtr)SC.MOUSEMOVE, IntPtr.Zero);
                    // NativeMethods.SendMessage(criticalHandle, WM.LBUTTONUP, IntPtr.Zero, IntPtr.Zero);

                    var wpfPoint = PointToScreen(Mouse.GetPosition(this));
                    var x = (int)wpfPoint.X;
                    var y = (int)wpfPoint.Y;
                    PInvoke.SendMessage(new HWND(criticalHandle), 0x00A1 /*NCLBUTTONDOWN*/, new WPARAM((nuint)HT.CAPTION), new LPARAM(x | (y << 16)));
                }
            }
        }

        private static readonly PropertyInfo criticalHandlePropertyInfo = typeof(Window).GetProperty("CriticalHandle", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly object[] emptyObjectArray = Array.Empty<object>();

        private ImageSource GetDefaultIcon(Size desiredSize)
        {
            IntPtr windowHandle;
            if((windowHandle = new WindowInteropHelper(this).Handle) != IntPtr.Zero)
            {
                return GetDefaultIcon(windowHandle, desiredSize);
            }

            return null;
        }

        private unsafe ImageSource GetDefaultIcon(IntPtr hwnd, Size desiredSize)
        {
#pragma warning disable CS0219 // Variable is assigned but its value is never used

            // Retrieve the small icon for the window.
            const int ICON_SMALL = 0;
            // Retrieve the large icon for the window.
            const int ICON_BIG = 1;
            // Retrieves the small icon provided by the application. If the application does not provide one, the system uses the system-generated icon for that window.
            const int ICON_SMALL2 = 2;

            const int IDI_APPLICATION = 0x7f00;

#pragma warning restore CS0219 // Variable is assigned but its value is never used

            try
            {
#pragma warning disable 618
                var iconPtr = IntPtr.Zero;

                if (hwnd != IntPtr.Zero)
                {
                    iconPtr = PInvoke.SendMessage(new HWND(hwnd), 0x007F /*GETICON*/, new(ICON_SMALL2), IntPtr.Zero);

                    if (iconPtr == IntPtr.Zero)
                    {
                        iconPtr = new IntPtr(PInvoke.GetClassLong(new HWND(hwnd), GET_CLASS_LONG_INDEX.GCLP_HICONSM));
                    }
                }

                if (iconPtr == IntPtr.Zero)
                {
                    var lpNameLocal = (char*)IDI_APPLICATION;
                    iconPtr = PInvoke.LoadImage(default, lpNameLocal, GDI_IMAGE_TYPE.IMAGE_ICON, (int)desiredSize.Width, (int)desiredSize.Height, IMAGE_FLAGS.LR_SHARED);
                }

                if (iconPtr != IntPtr.Zero)
                {
                    var bitmapFrame = BitmapFrame.Create(Imaging.CreateBitmapSourceFromHIcon(iconPtr, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight((int)desiredSize.Width, (int)desiredSize.Height)));
                    return bitmapFrame;
                }
#pragma warning restore 618
            }
            catch
            {
                // ignored
            }

            return null;
        }
    }
}
