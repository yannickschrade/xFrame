using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace xFrame.WPF.Controls.Windows
{
    internal class DefaultHeader : Control
    {

        private Window _ownerWindow;

        public ImageSource IconSource
        {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(DefaultHeader), new PropertyMetadata(default));


        static DefaultHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DefaultHeader), new FrameworkPropertyMetadata(typeof(WindowCommands)));
        }

        public DefaultHeader()
        {
            Loaded += (s, e) =>
             {
                 _ownerWindow = Window.GetWindow(this);
                 ImageSource source = _ownerWindow.Icon;
                 if (source == null)
                     source = GetDefaultIcon(new Size(32, 32));

                 IconSource = source;
             };
        }

        private ImageSource GetDefaultIcon(Size desiredSize)
        {
            IntPtr windowHandle;
            if ((windowHandle = new WindowInteropHelper(_ownerWindow).Handle) != IntPtr.Zero)
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
