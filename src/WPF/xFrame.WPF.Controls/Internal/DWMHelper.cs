using System;
using global::Windows.Win32;
using global::Windows.Win32.Foundation;
using global::Windows.Win32.Graphics.Dwm;
using global::Windows.Win32.UI.Controls;

namespace xFrame.WPF.Controls.Internal
{
    internal static class DwmHelper
    {
        public static unsafe bool IsCompositionEnabled()
        {
            BOOL pfEnabled;
            var result = PInvoke.DwmIsCompositionEnabled(&pfEnabled);
            return pfEnabled == true;
        }

        public static bool SetWindowAttributeValue(IntPtr hWnd, DWMWINDOWATTRIBUTE attribute, int attributeValue)
        {
            return SetWindowAttribute(hWnd, attribute, ref attributeValue);
        }

        public static unsafe bool SetWindowAttribute(IntPtr hWnd, DWMWINDOWATTRIBUTE attribute, ref int attributeValue)
        {
            fixed (void* value = &attributeValue)
            {
                var result = PInvoke.DwmSetWindowAttribute(new HWND(hWnd), attribute, value, sizeof(int));
                return result == 0;
            }
        }

        public static unsafe bool WindowExtendIntoClientArea(IntPtr hWnd, MARGINS margins)
        {
            // Extend frame on the bottom of client area
            var result = PInvoke.DwmExtendFrameIntoClientArea(new HWND(hWnd), &margins);
            return result == 0;
        }
    }
}