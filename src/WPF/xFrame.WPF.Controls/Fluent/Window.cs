using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Windows.Win32;
using xFrame.WPF.Controls.Native;
using xFrame.WPF.Controls.Windows;
using Windows.Win32.Foundation;
using xFrame.WPF.Controls.WindowShell;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;
using System.Linq;
using System.Windows.Media;

namespace xFrame.WPF.Controls.Fluent
{
    public static class Window
    {
        private static readonly object[] emptyObjectArray = Array.Empty<object>();

        public static readonly DependencyProperty WindowCommandsProperty = DependencyProperty.RegisterAttached(
            "WindowCommands", typeof(FrameworkElement), typeof(Window),
            new FrameworkPropertyMetadata(null));

        public static FrameworkElement GetWindowCommands(System.Windows.Window window)
            => (FrameworkElement)window.GetValue(WindowCommandsProperty);

        public static void SetWindowCommands(System.Windows.Window window, FrameworkElement commands)
            => window.SetValue(WindowCommandsProperty, commands);


        public static readonly DependencyProperty WindowHeaderProperty = DependencyProperty.RegisterAttached(
            "WindowHeader", typeof(FrameworkElement), typeof(Window),
            new FrameworkPropertyMetadata(null, OnHeaderChanged));

        public static FrameworkElement GetWindowHeader(System.Windows.Window window)
            => (FrameworkElement)window.GetValue(WindowHeaderProperty);

        public static void SetWindowHeader(System.Windows.Window window, FrameworkElement commands)
            => window.SetValue(WindowHeaderProperty, commands);

        private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)e.NewValue;

            element.MouseLeftButtonDown += (s, e) =>
             {
                 var win = (System.Windows.Window)d;
                 var criticalHandlePropertyInfo = d.GetType().GetProperty("CriticalHandle", BindingFlags.NonPublic | BindingFlags.Instance);
                 if (e.ClickCount == 1)
                 {
                     e.Handled = true;
                     element.VerifyAccess();


                     PInvoke.ReleaseCapture();
                     var criticalHandle = (IntPtr)(criticalHandlePropertyInfo?.GetValue(win, emptyObjectArray) ?? IntPtr.Zero);

                     if (criticalHandle != IntPtr.Zero)
                     {
                         // these lines are from DragMove
                         // NativeMethods.SendMessage(criticalHandle, WM.SYSCOMMAND, (IntPtr)SC.MOUSEMOVE, IntPtr.Zero);
                         // NativeMethods.SendMessage(criticalHandle, WM.LBUTTONUP, IntPtr.Zero, IntPtr.Zero);

                         var wpfPoint = win.PointToScreen(Mouse.GetPosition(win));
                         var x = (int)wpfPoint.X;
                         var y = (int)wpfPoint.Y;
                         PInvoke.SendMessage(new HWND(criticalHandle), 0x00A1 /*NCLBUTTONDOWN*/, new WPARAM((nuint)HT.CAPTION), new LPARAM(x | (y << 16)));
                     }
                 }
             };

        }



        public static bool GetUseDefaultHeader(System.Windows.Window obj)
        {
            return (bool)obj.GetValue(UseDefaultHeaderProperty);
        }

        public static void SetUseDefaultHeader(System.Windows.Window obj, bool value)
        {
            obj.SetValue(UseDefaultHeaderProperty, value);
        }

        // Using a DependencyProperty as the backing store for UseDefaultHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseDefaultHeaderProperty =
            DependencyProperty.RegisterAttached("UseDefaultHeader", typeof(bool), typeof(Window), new PropertyMetadata(false, OnUseDefaultHeaderChanged));

        private static void OnUseDefaultHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (bool)e.NewValue;
            var window = (System.Windows.Window)d;

            if (!value)
                return;

            SetWindowHeader(window, new DefaultHeader());
        }



        public static bool GetUseDefaultSystemCommands(DependencyObject obj)
        {
            return (bool)obj.GetValue(UseDefaultSystemCommandsProperty);
        }

        public static void SetUseDefaultSystemCommands(DependencyObject obj, bool value)
        {
            obj.SetValue(UseDefaultSystemCommandsProperty, value);
        }

        // Using a DependencyProperty as the backing store for UseDefaultSystemCommands.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseDefaultSystemCommandsProperty =
            DependencyProperty.RegisterAttached("UseDefaultSystemCommands", typeof(bool), typeof(Window), new PropertyMetadata(false, OnUseDefaultCommandsChanged));

        private static void OnUseDefaultCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (bool)e.NewValue;
            var window = (System.Windows.Window)d;

            if (!value)
                return;

            SetWindowCommands(window, new WindowCommands());
        }

        public static bool GetShowMinButton(System.Windows.Window obj)
        {
            return (bool)obj.GetValue(ShowMinButtonProperty);
        }

        public static void SetShowMinButton(System.Windows.Window obj, bool value)
        {
            obj.SetValue(ShowMinButtonProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowMinButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowMinButtonProperty =
            DependencyProperty.RegisterAttached("ShowMinButton", typeof(bool), typeof(Window), new PropertyMetadata(true));




        public static bool GetShowMaxRestoreButton(System.Windows.Window obj)
        {
            return (bool)obj.GetValue(ShowMaxRestoreButtonProperty);
        }

        public static void SetShowMaxRestoreButton(System.Windows.Window obj, bool value)
        {
            obj.SetValue(ShowMaxRestoreButtonProperty, value);
        }

        // Using a DependencyProperty as the backing store for ShowMaxRestoreButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowMaxRestoreButtonProperty =
            DependencyProperty.RegisterAttached("ShowMaxRestoreButton", typeof(bool), typeof(Window), new PropertyMetadata(true));



        #region WindowChromeBehaviorProperties

        public static readonly DependencyProperty ResizeBorderThicknessProperty =
            DependencyProperty.RegisterAttached("RisizeBorderThickness", typeof(Thickness), typeof(Window),
                new PropertyMetadata(WindowChromeBehavior.ResizeBorderThicknessProperty.DefaultMetadata.DefaultValue));

        public static Thickness GetRisizeBorderThickness(System.Windows.Window obj)
        {
            return (Thickness)obj.GetValue(ResizeBorderThicknessProperty);
        }

        public static void SetRisizeBorderThickness(System.Windows.Window obj, Thickness value)
        {
            obj.SetValue(ResizeBorderThicknessProperty, value);
        }




        public static bool GetIgnoreTaskbarOnMaximize(System.Windows.Window obj)
        {
            return (bool)obj.GetValue(IgnoreTaskbarOnMaximizeProperty);
        }

        public static void SetIgnoreTaskbarOnMaximize(System.Windows.Window obj, bool value)
        {
            obj.SetValue(IgnoreTaskbarOnMaximizeProperty, value);
        }

        // Using a DependencyProperty as the backing store for IgnorTaskbarOnMaximized.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreTaskbarOnMaximizeProperty =
            DependencyProperty.RegisterAttached("IgnoreTaskbarOnMaximize", typeof(bool), typeof(Window), new PropertyMetadata(WindowChromeBehavior.IgnoreTaskbarOnMaximizeProperty.DefaultMetadata.DefaultValue));




        public static bool GetKeepBorderOnMaximize(System.Windows.Window obj)
        {
            return (bool)obj.GetValue(KeepBorderOnMaximizeProperty);
        }

        public static void SetKeepBorderOnMaximize(System.Windows.Window obj, bool value)
        {
            obj.SetValue(KeepBorderOnMaximizeProperty, value);
        }

        // Using a DependencyProperty as the backing store for KeepBoarderOnMaximize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeepBorderOnMaximizeProperty =
            DependencyProperty.RegisterAttached("KeepBorderOnMaximize", typeof(bool), typeof(Window), new PropertyMetadata(WindowChromeBehavior.KeepBorderOnMaximizeProperty.DefaultMetadata.DefaultValue));





        public static bool GetIsNCActive(System.Windows.Window obj)
        {
            return (bool)obj.GetValue(IsNCActiveProperty);
        }

        public static void SetIsNCActive(System.Windows.Window obj, bool value)
        {
            obj.SetValue(IsNCActiveProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsNCActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNCActiveProperty =
            DependencyProperty.RegisterAttached("IsNCActive", typeof(bool), typeof(Window), new PropertyMetadata(WindowChromeBehavior.IsNCActiveProperty.DefaultMetadata.DefaultValue));





        public static WindowCornerPreference GetCornerPreference(System.Windows.Window obj)
        {
            return (WindowCornerPreference)obj.GetValue(CornerPreferenceProperty);
        }

        public static void SetCornerPreference(System.Windows.Window obj, WindowCornerPreference value)
        {
            obj.SetValue(CornerPreferenceProperty, value);
        }

        // Using a DependencyProperty as the backing store for CornerPreference.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerPreferenceProperty =
            DependencyProperty.RegisterAttached("CornerPreference", typeof(WindowCornerPreference), typeof(Window), new PropertyMetadata(WindowChromeBehavior.CornerPreferenceProperty.DefaultMetadata.DefaultValue));



        #endregion



        #region GlowWindowBehaviorProperties

        // Using a DependencyProperty as the backing store for GlowDepth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GlowDepthProperty =
            DependencyProperty.RegisterAttached("GlowDepth", typeof(int), typeof(Window), new PropertyMetadata(GlowWindowBehavior.GlowDepthProperty.DefaultMetadata.DefaultValue));

        public static int GetGlowDepth(System.Windows.Window obj)
        {
            return (int)obj.GetValue(GlowDepthProperty);
        }

        public static void SetGlowDepth(System.Windows.Window obj, int value)
        {
            obj.SetValue(GlowDepthProperty, value);
        }



        public static bool GetUseRadialGradientForCorners(System.Windows.Window obj)
        {
            return (bool)obj.GetValue(UseRadialGradientForCornersProperty);
        }

        public static void SetUseRadialGradientForCorners(System.Windows.Window obj, bool value)
        {
            obj.SetValue(UseRadialGradientForCornersProperty, value);
        }

        // Using a DependencyProperty as the backing store for UseRadialGradientForCorners.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseRadialGradientForCornersProperty =
            DependencyProperty.RegisterAttached("UseRadialGradientForCorners", typeof(bool), typeof(Window), new PropertyMetadata(GlowWindowBehavior.UseRadialGradientForCornersProperty.DefaultMetadata.DefaultValue));



        public static bool GetIsGlowTransitionEnabled(System.Windows.Window obj)
        {
            return (bool)obj.GetValue(IsGlowTransitionEnabledProperty);
        }

        public static void SetIsGlowTransitionEnabled(System.Windows.Window obj, bool value)
        {
            obj.SetValue(IsGlowTransitionEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsGlowTransitionEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsGlowTransitionEnabledProperty =
            DependencyProperty.RegisterAttached("IsGlowTransitionEnabled", typeof(bool), typeof(Window), new PropertyMetadata(GlowWindowBehavior.IsGlowTransitionEnabledProperty.DefaultMetadata.DefaultValue));




        public static Color? GetGlowColor(System.Windows.Window obj)
        {
            return (Color?)obj.GetValue(GlowColorProperty);
        }

        public static void SetGlowColor(System.Windows.Window obj, Color? value)
        {
            obj.SetValue(GlowColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for GlowColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GlowColorProperty =
            DependencyProperty.RegisterAttached("GlowColor", typeof(Color?), typeof(Window), new PropertyMetadata(null));



        public static Color? GetNonActiveGlowColor(System.Windows.Window obj)
        {
            return (Color?)obj.GetValue(NonActiveGlowColorProperty);
        }

        public static void SetNonActiveGlowColor(System.Windows.Window obj, Color? value)
        {
            obj.SetValue(NonActiveGlowColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for NonActiveGlowColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NonActiveGlowColorProperty =
            DependencyProperty.RegisterAttached("NonActiveGlowColor", typeof(Color?), typeof(Window), new PropertyMetadata(null));





        public static bool GetPreferDWMBorderColor(System.Windows.Window obj)
        {
            return (bool)obj.GetValue(PreferDWMBorderColorProperty);
        }

        public static void SetPreferDWMBorderColor(System.Windows.Window obj, bool value)
        {
            obj.SetValue(PreferDWMBorderColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for PreferDWMBorderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreferDWMBorderColorProperty =
            DependencyProperty.RegisterAttached("PreferDWMBorderColor", typeof(bool), typeof(Window), new PropertyMetadata(true));




        public static bool GetDWMSupportsBorderColor(System.Windows.Window obj)
        {
            return (bool)obj.GetValue(DWMSupportsBorderColorProperty);
        }

        public static void SetDWMSupportsBorderColor(System.Windows.Window obj, bool value)
        {
            obj.SetValue(DWMSupportsBorderColorProperty, value);
        }

        // Using a DependencyProperty as the backing store for DWMSupportsBorderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DWMSupportsBorderColorProperty =
            DependencyProperty.RegisterAttached("DWMSupportsBorderColor", typeof(bool), typeof(Window), new PropertyMetadata(false));



        #endregion





        // Using a DependencyProperty as the backing store for AttachShellBehaviors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttachShellBehaviorsProperty =
            DependencyProperty.RegisterAttached("AttachShellBehaviors", typeof(bool), typeof(Window), new PropertyMetadata(false, OnAttachShellBehaviorsChanged));

        public static bool GetAttachShellBehaviors(System.Windows.Window obj)
        {
            return (bool)obj.GetValue(AttachShellBehaviorsProperty);
        }

        public static void SetAttachShellBehaviors(System.Windows.Window obj, bool value)
        {
            obj.SetValue(AttachShellBehaviorsProperty, value);
        }

        private static void OnAttachShellBehaviorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = (System.Windows.Window)d;
            var value = (bool)e.NewValue;
            if (value)
            {
                var chromeBehavior = new WindowChromeBehavior();
                BindingOperations.SetBinding(chromeBehavior, WindowChromeBehavior.ResizeBorderThicknessProperty, new Binding { Path = new PropertyPath(ResizeBorderThicknessProperty), Source = window });
                BindingOperations.SetBinding(chromeBehavior, WindowChromeBehavior.IgnoreTaskbarOnMaximizeProperty, new Binding { Path = new PropertyPath(IgnoreTaskbarOnMaximizeProperty), Source = window });
                BindingOperations.SetBinding(chromeBehavior, WindowChromeBehavior.KeepBorderOnMaximizeProperty, new Binding { Path = new PropertyPath(KeepBorderOnMaximizeProperty), Source = window });
                BindingOperations.SetBinding(chromeBehavior, WindowChromeBehavior.EnableMinimizeProperty, new Binding { Path = new PropertyPath(ShowMinButtonProperty), Source = window });
                BindingOperations.SetBinding(chromeBehavior, WindowChromeBehavior.EnableMaxRestoreProperty, new Binding { Path = new PropertyPath(ShowMaxRestoreButtonProperty), Source = window });
                BindingOperations.SetBinding(chromeBehavior, WindowChromeBehavior.CornerPreferenceProperty, new Binding { Path = new PropertyPath(CornerPreferenceProperty), Source = window });

                var glowBehavior = new GlowWindowBehavior();
                BindingOperations.SetBinding(glowBehavior, GlowWindowBehavior.GlowDepthProperty, new Binding { Path = new PropertyPath(GlowDepthProperty), Source = window });
                BindingOperations.SetBinding(glowBehavior, GlowWindowBehavior.GlowColorProperty, new Binding { Path = new PropertyPath(GlowColorProperty), Source = window });
                BindingOperations.SetBinding(glowBehavior, GlowWindowBehavior.NonActiveGlowColorProperty, new Binding { Path = new PropertyPath(NonActiveGlowColorProperty), Source = window });
                BindingOperations.SetBinding(glowBehavior, GlowWindowBehavior.UseRadialGradientForCornersProperty, new Binding { Path = new PropertyPath(UseRadialGradientForCornersProperty), Source = window });
                BindingOperations.SetBinding(glowBehavior, GlowWindowBehavior.IsGlowTransitionEnabledProperty, new Binding { Path = new PropertyPath(IsGlowTransitionEnabledProperty), Source = window });
                BindingOperations.SetBinding(glowBehavior, GlowWindowBehavior.PreferDWMBorderColorProperty, new Binding { Path = new PropertyPath(PreferDWMBorderColorProperty), Source = window });

                var behaviors = Interaction.GetBehaviors(window);
                behaviors.Add(chromeBehavior);
                behaviors.Add(glowBehavior);
            }
            else
            {
                var behaviors = Interaction.GetBehaviors(window);
                var windowChromeBehavior = behaviors.FirstOrDefault(b => b is WindowChromeBehavior);
                var glowWindowBehavior = behaviors.FirstOrDefault(b => b is GlowWindowBehavior);
                behaviors.Remove(windowChromeBehavior);
                behaviors.Remove(glowWindowBehavior);
            }
        }





        // Using a DependencyProperty as the backing store for HeaderHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.RegisterAttached("HeaderHeight", typeof(double), typeof(Window), new PropertyMetadata(double.NaN));

        public static double GetHeaderHeight(System.Windows.Window obj)
        {
            return (double)obj.GetValue(HeaderHeightProperty);
        }

        public static void SetHeaderHeight(System.Windows.Window obj, double value)
        {
            obj.SetValue(HeaderHeightProperty, value);
        }



    }
}
