//Code from controlzEx library https://github.com/ControlzEx/ControlzEx

using System;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

namespace xFrame.WPF.Controls.WindowShell
{
    public partial class WindowChromeBehavior
    {
        private class SuppressRedrawScope : IDisposable
        {
            private readonly HWND hwnd;

            private readonly bool suppressedRedraw;

            public SuppressRedrawScope(IntPtr hwnd)
            {
                this.hwnd = new HWND(hwnd);

                if ((PInvoke.GetWindowStyle(this.hwnd) & WINDOW_STYLE.WS_VISIBLE) != 0)
                {
                    this.SetRedraw(state: false);
                    this.suppressedRedraw = true;
                }
            }

            public unsafe void Dispose()
            {
                if (this.suppressedRedraw)
                {
                    this.SetRedraw(state: true);
                    const REDRAW_WINDOW_FLAGS FLAGS = REDRAW_WINDOW_FLAGS.RDW_INVALIDATE | REDRAW_WINDOW_FLAGS.RDW_ALLCHILDREN | REDRAW_WINDOW_FLAGS.RDW_FRAME;
                    PInvoke.RedrawWindow(this.hwnd, default, null, FLAGS);
                }
            }

            private void SetRedraw(bool state)
            {
                PInvoke.SendMessage(this.hwnd, WM.SETREDRAW, (nuint)Convert.ToInt32(state), default);
            }
        }
    }
}