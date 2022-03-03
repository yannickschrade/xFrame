using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using global::Windows.Win32;
using global::Windows.Win32.Foundation;
using global::Windows.Win32.Graphics.Gdi;
using global::Windows.Win32.UI.WindowsAndMessaging;
using xFrame.WPF.Controls.Helper;
using xFrame.WPF.Controls.Native;

namespace xFrame.WPF.Controls.WindowShell
{
#pragma warning disable 618, SA1602, SA1401

    public abstract class HwndWrapper : DisposableObject
    {
        private HWND hwnd;
        private IntPtr handle;

        private bool isHandleCreationAllowed = true;

        private WNDPROC? wndProc;

        public abstract string ClassName { get; }

        public static int LastDestroyWindowError { get; private set; }

        [CLSCompliant(false)]
        protected ushort WindowClassAtom { get; private set; }

        public IntPtr Handle
        {
            get
            {
                EnsureHandle();
                return handle;
            }
        }

        internal HWND Hwnd
        {
            get
            {
                EnsureHandle();
                return hwnd;
            }
        }

        protected virtual bool IsWindowSubClassed => false;

        [CLSCompliant(false)]
        protected virtual ushort CreateWindowClassCore()
        {
            return RegisterClass(ClassName);
        }

        protected virtual void DestroyWindowClassCore()
        {
            if (WindowClassAtom != 0)
            {
                var moduleHandle = PInvoke.GetModuleHandle((string?)null);
                PInvoke.UnregisterClass(ClassName, moduleHandle);
                WindowClassAtom = 0;
            }
        }

        [CLSCompliant(false)]
        protected unsafe ushort RegisterClass(string className)
        {
            wndProc = WndProcWrapper;

            fixed (char* cls = className)
            {
                var lpWndClass = new WNDCLASSEXW
                {
                    cbSize = (uint)Marshal.SizeOf(typeof(WNDCLASSEXW)),
                    hInstance = PInvoke.GetModuleHandle((PCWSTR)null),
                    lpfnWndProc = wndProc,
                    lpszClassName = cls,
                };

                var atom = PInvoke.RegisterClassEx(lpWndClass);

                return atom;
            }
        }

        private void SubclassWndProc()
        {
            wndProc = WndProcWrapper;
            PInvoke.SetWindowLongPtr(Hwnd, WINDOW_LONG_PTR_INDEX.GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(wndProc));
        }

        protected abstract IntPtr CreateWindowCore();

        protected virtual void DestroyWindowCore()
        {
            if (handle != IntPtr.Zero)
            {
                if (PInvoke.DestroyWindow(hwnd) == false)
                {
                    LastDestroyWindowError = Marshal.GetLastWin32Error();
                }

                handle = default;
                hwnd = default;
            }
        }

        private LRESULT WndProcWrapper(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
        {
            return new LRESULT(WndProc(hwnd, msg, wParam, lParam));
        }

        [CLSCompliant(false)]
        protected virtual nint WndProc(nint hwnd, uint msg, nuint wParam, nint lParam)
        {
            return PInvoke.DefWindowProc(new HWND(hwnd), msg, wParam, lParam);
        }

        public IntPtr EnsureHandle()
        {
            if (handle != IntPtr.Zero)
            {
                return handle;
            }

            if (isHandleCreationAllowed == false)
            {
                return IntPtr.Zero;
            }

            if (IsDisposed)
            {
                return IntPtr.Zero;
            }

            isHandleCreationAllowed = false;
            WindowClassAtom = CreateWindowClassCore();
            handle = CreateWindowCore();

            if (IsWindowSubClassed)
            {
                SubclassWndProc();
            }

            return handle;
        }

        protected override void DisposeNativeResources()
        {
            isHandleCreationAllowed = false;
            DestroyWindowCore();
            DestroyWindowClassCore();
        }
    }

    public class DisposableObject : IDisposable
    {
        private EventHandler disposingEventHandlers;

        public bool IsDisposing { get; private set; }

        public bool IsDisposed { get; private set; }

        public event EventHandler Disposing
        {
            add
            {
                ThrowIfDisposed();
                disposingEventHandlers = (EventHandler)Delegate.Combine(disposingEventHandlers, value);
            }
            remove => disposingEventHandlers = (EventHandler)Delegate.Remove(disposingEventHandlers, value);
        }

        ~DisposableObject()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed
                || IsDisposing)
            {
                return;
            }

            IsDisposing = true;

            try
            {
                if (disposing)
                {
                    disposingEventHandlers?.Invoke(this, EventArgs.Empty);
                    disposingEventHandlers = null;
                    DisposeManagedResources();
                }

                DisposeNativeResources();
            }
            finally
            {
                IsDisposed = true;
                IsDisposing = false;
            }
        }

        protected virtual void DisposeManagedResources()
        {
        }

        protected virtual void DisposeNativeResources()
        {
        }
    }

    public sealed class GlowBitmap : DisposableObject
    {
        private sealed class CachedBitmapInfo
        {
            public readonly int Width;

            public readonly int Height;

            public readonly byte[] DiBits;

            public CachedBitmapInfo(byte[] diBits, int width, int height)
            {
                Width = width;
                Height = height;
                DiBits = diBits;
            }
        }

        public const int GlowBitmapPartCount = 16;

        private const int BytesPerPixelBgra32 = 4;

        private static readonly Dictionary<CachedBitmapInfoKey, CachedBitmapInfo?[]> transparencyMasks = new();

        private IntPtr pbits;

        private readonly BITMAPINFO bitmapInfo;

        public SafeHandle Handle { get; }

        public IntPtr DiBits => pbits;

        public int Width => bitmapInfo.bmiHeader.biWidth;

        public int Height => -bitmapInfo.bmiHeader.biHeight;

        public unsafe GlowBitmap(SafeHandle hdcScreen, int width, int height)
        {
            bitmapInfo.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            bitmapInfo.bmiHeader.biPlanes = 1;
            bitmapInfo.bmiHeader.biBitCount = 32;
            bitmapInfo.bmiHeader.biCompression = 0;
            bitmapInfo.bmiHeader.biXPelsPerMeter = 0;
            bitmapInfo.bmiHeader.biYPelsPerMeter = 0;
            bitmapInfo.bmiHeader.biWidth = width;
            bitmapInfo.bmiHeader.biHeight = -height;

            fixed (BITMAPINFO* pbitmapinfo = &bitmapInfo)
            {
                Handle = new DeleteObjectSafeHandle(PInvoke.CreateDIBSection(new HDC(hdcScreen.DangerousGetHandle()), pbitmapinfo, DIB_USAGE.DIB_RGB_COLORS, out var bits, default, 0));
                pbits = bits;
            }
        }

        protected override void DisposeNativeResources()
        {
            Handle.Dispose();
        }

        private static byte PremultiplyAlpha(byte channel, byte alpha)
        {
            return (byte)(channel * alpha / 255.0);
        }

        public static GlowBitmap? Create(GlowDrawingContext drawingContext, GlowBitmapPart bitmapPart, Color color, int glowDepth, bool useRadialGradientForCorners)
        {
            if (drawingContext.ScreenDc is null)
            {
                return null;
            }

            var alphaMask = GetOrCreateAlphaMask(bitmapPart, glowDepth, useRadialGradientForCorners);
            var glowBitmap = new GlowBitmap(drawingContext.ScreenDc, alphaMask.Width, alphaMask.Height);
            for (var i = 0; i < alphaMask.DiBits.Length; i += BytesPerPixelBgra32)
            {
                var b = alphaMask.DiBits[i + 3];
                var val = PremultiplyAlpha(color.R, b);
                var val2 = PremultiplyAlpha(color.G, b);
                var val3 = PremultiplyAlpha(color.B, b);
                Marshal.WriteByte(glowBitmap.DiBits, i, val3);
                Marshal.WriteByte(glowBitmap.DiBits, i + 1, val2);
                Marshal.WriteByte(glowBitmap.DiBits, i + 2, val);
                Marshal.WriteByte(glowBitmap.DiBits, i + 3, b);
            }

            return glowBitmap;
        }

        private static CachedBitmapInfo GetOrCreateAlphaMask(GlowBitmapPart bitmapPart, int glowDepth, bool useRadialGradientForCorners)
        {
            var cacheKey = new CachedBitmapInfoKey(glowDepth, useRadialGradientForCorners);
            if (transparencyMasks.TryGetValue(cacheKey, out var transparencyMasksForGlowDepth) == false)
            {
                transparencyMasksForGlowDepth = new CachedBitmapInfo?[GlowBitmapPartCount];
                transparencyMasks[cacheKey] = transparencyMasksForGlowDepth;
            }

            var num = (int)bitmapPart;
            if (transparencyMasksForGlowDepth[num] is { } transparencyMask)
            {
                return transparencyMask;
            }

            var bitmapImage = GlowWindowBitmapGenerator.GenerateBitmapSource(bitmapPart, glowDepth, useRadialGradientForCorners);
            var array = new byte[BytesPerPixelBgra32 * bitmapImage.PixelWidth * bitmapImage.PixelHeight];
            var stride = BytesPerPixelBgra32 * bitmapImage.PixelWidth;
            bitmapImage.CopyPixels(array, stride, 0);
            var cachedBitmapInfo = new CachedBitmapInfo(array, bitmapImage.PixelWidth, bitmapImage.PixelHeight);
            transparencyMasksForGlowDepth[num] = cachedBitmapInfo;

            return cachedBitmapInfo;
        }
    }

    internal readonly struct CachedBitmapInfoKey : IEquatable<CachedBitmapInfoKey>
    {
        public CachedBitmapInfoKey(int glowDepth, bool useRadialGradientForCorners)
        {
            GlowDepth = glowDepth;
            UseRadialGradientForCorners = useRadialGradientForCorners;
        }

        public int GlowDepth { get; }

        public bool UseRadialGradientForCorners { get; }

        public bool Equals(CachedBitmapInfoKey other)
        {
            return GlowDepth == other.GlowDepth && UseRadialGradientForCorners == other.UseRadialGradientForCorners;
        }

        public override bool Equals(object? obj)
        {
            return obj is CachedBitmapInfoKey other
                   && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return GlowDepth * 397 ^ UseRadialGradientForCorners.GetHashCode();
            }
        }

        public static bool operator ==(CachedBitmapInfoKey left, CachedBitmapInfoKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CachedBitmapInfoKey left, CachedBitmapInfoKey right)
        {
            return !left.Equals(right);
        }
    }

    public enum GlowBitmapPart
    {
        CornerTopLeft,
        CornerTopRight,
        CornerBottomLeft,
        CornerBottomRight,
        TopLeft,
        Top,
        TopRight,
        LeftTop,
        Left,
        LeftBottom,
        BottomLeft,
        Bottom,
        BottomRight,
        RightTop,
        Right,
        RightBottom
    }

    public sealed class GlowDrawingContext : DisposableObject
    {
        internal BLENDFUNCTION Blend;

        private readonly GlowBitmap? windowBitmap;

        [MemberNotNullWhen(true, nameof(ScreenDc))]
        [MemberNotNullWhen(true, nameof(WindowDc))]
        [MemberNotNullWhen(true, nameof(BackgroundDc))]
        [MemberNotNullWhen(true, nameof(windowBitmap))]
        public bool IsInitialized
        {
            get
            {
                if (ScreenDc is null
                    || WindowDc is null
                    || BackgroundDc is null
                    || windowBitmap is null)
                {
                    return false;
                }

                if (ScreenDc.DangerousGetHandle() != IntPtr.Zero
                    && WindowDc.DangerousGetHandle() != IntPtr.Zero
                    && BackgroundDc.DangerousGetHandle() != IntPtr.Zero)
                {
                    return windowBitmap is not null;
                }

                return false;
            }
        }

        public SafeHandle? ScreenDc { get; private set; }

        public SafeHandle? WindowDc { get; }

        public SafeHandle? BackgroundDc { get; }

        public int Width => windowBitmap?.Width ?? 0;

        public int Height => windowBitmap?.Height ?? 0;

        private static SafeHandle? desktopDC;

        public GlowDrawingContext(int width, int height)
        {
            SetupDesktopDC();

            if (ScreenDc is null)
            {
                return;
            }

            try
            {
                WindowDc = PInvoke.CreateCompatibleDC(ScreenDc);
            }
            catch
            {
                desktopDC?.Dispose();
                desktopDC = null;
                SetupDesktopDC();

                WindowDc = PInvoke.CreateCompatibleDC(ScreenDc);
            }

            if (WindowDc.DangerousGetHandle() == IntPtr.Zero)
            {
                return;
            }

            BackgroundDc = PInvoke.CreateCompatibleDC(ScreenDc);

            if (BackgroundDc.DangerousGetHandle() == IntPtr.Zero)
            {
                return;
            }

            Blend.BlendOp = 0;
            Blend.BlendFlags = 0;
            Blend.SourceConstantAlpha = byte.MaxValue;
            Blend.AlphaFormat = 0x01; // AC_SRC_ALPHA;
            windowBitmap = new GlowBitmap(ScreenDc, width, height);
            PInvoke.SelectObject(WindowDc, windowBitmap.Handle);
        }

        private void SetupDesktopDC()
        {
            desktopDC ??= new DeleteDCSafeHandle(PInvoke.GetDC(default));

            ScreenDc = desktopDC;
            if (ScreenDc.DangerousGetHandle() == IntPtr.Zero)
            {
                ScreenDc?.Dispose();
                ScreenDc = null;
            }
        }

        protected override void DisposeManagedResources()
        {
            windowBitmap?.Dispose();
        }

        protected override void DisposeNativeResources()
        {
            WindowDc?.Dispose();

            BackgroundDc?.Dispose();
        }
    }

    [CLSCompliant(false)]
    public sealed class GlowWindow : HwndWrapper, IGlowWindow
    {
        [Flags]
        private enum FieldInvalidationTypes
        {
            None = 0,
            Location = 1 << 1,
            Size = 1 << 2,
            ActiveColor = 1 << 3,
            InactiveColor = 1 << 4,
            Render = 1 << 5,
            Visibility = 1 << 6,
            GlowDepth = 1 << 7
        }

        private readonly Window targetWindow;
        private readonly GlowWindowBehavior behavior;

        private readonly Dock orientation;

        private readonly GlowBitmap[] activeGlowBitmaps = new GlowBitmap[GlowBitmap.GlowBitmapPartCount];

        private readonly GlowBitmap[] inactiveGlowBitmaps = new GlowBitmap[GlowBitmap.GlowBitmapPartCount];

        private static ushort sharedWindowClassAtom;

        // Member to keep reference alive
        // ReSharper disable NotAccessedField.Local
#pragma warning disable IDE0052 // Remove unread private members
        private static WNDPROC sharedWndProc;
#pragma warning restore IDE0052 // Remove unread private members
        // ReSharper restore NotAccessedField.Local

        private int left;

        private int top;

        private int width;

        private int height;

        private int glowDepth = 9;
        private readonly int cornerGripThickness = Constants.ResizeCornerGripThickness;

        private bool useRadialGradientForCorners = true;

        private bool isVisible;

        private bool isActive;

        private Color activeGlowColor = Colors.Transparent;

        private Color inactiveGlowColor = Colors.Transparent;

        private FieldInvalidationTypes invalidatedValues;

        private bool pendingDelayRender;
        private string title;

#pragma warning disable SA1310
        private static readonly LPARAM SW_PARENTCLOSING = new(1);
        private static readonly LPARAM SW_PARENTOPENING = new(3);
#pragma warning restore SA1310

        private bool IsDeferringChanges => behavior.DeferGlowChangesCount > 0;

        private unsafe ushort SharedWindowClassAtom
        {
            get
            {
                if (sharedWindowClassAtom == 0)
                {
                    sharedWndProc ??= PInvoke.DefWindowProc;

                    fixed (char* cls = ClassName)
                    {
                        var lpWndClass = new WNDCLASSEXW
                        {
                            cbSize = (uint)Marshal.SizeOf(typeof(WNDCLASSEXW)),
                            hInstance = PInvoke.GetModuleHandle((PCWSTR)null),
                            lpfnWndProc = sharedWndProc,
                            lpszClassName = cls,
                        };

                        sharedWindowClassAtom = PInvoke.RegisterClassEx(lpWndClass);
                    }
                }

                return sharedWindowClassAtom;
            }
        }

        public override string ClassName { get; } = "ControlzEx_GlowWindow";

        public bool IsVisible
        {
            get => isVisible;
            set => UpdateProperty(ref isVisible, value, FieldInvalidationTypes.Render | FieldInvalidationTypes.Visibility);
        }

        public int Left
        {
            get => left;
            set => UpdateProperty(ref left, value, FieldInvalidationTypes.Location);
        }

        public int Top
        {
            get => top;
            set => UpdateProperty(ref top, value, FieldInvalidationTypes.Location);
        }

        public int Width
        {
            get => width;
            set => UpdateProperty(ref width, value, FieldInvalidationTypes.Size | FieldInvalidationTypes.Render);
        }

        public int Height
        {
            get => height;
            set => UpdateProperty(ref height, value, FieldInvalidationTypes.Size | FieldInvalidationTypes.Render);
        }

        public int GlowDepth
        {
            get => glowDepth;
            set => UpdateProperty(ref glowDepth, value, FieldInvalidationTypes.GlowDepth | FieldInvalidationTypes.Render | FieldInvalidationTypes.Location);
        }

        public bool UseRadialGradientForCorners
        {
            get => useRadialGradientForCorners;
            set => UpdateProperty(ref useRadialGradientForCorners, value, FieldInvalidationTypes.GlowDepth | FieldInvalidationTypes.Render | FieldInvalidationTypes.Location);
        }

        public bool IsActive
        {
            get => isActive;
            set => UpdateProperty(ref isActive, value, FieldInvalidationTypes.Render);
        }

        public Color ActiveGlowColor
        {
            get => activeGlowColor;
            set => UpdateProperty(ref activeGlowColor, value, FieldInvalidationTypes.ActiveColor | FieldInvalidationTypes.Render);
        }

        public Color InactiveGlowColor
        {
            get => inactiveGlowColor;
            set => UpdateProperty(ref inactiveGlowColor, value, FieldInvalidationTypes.InactiveColor | FieldInvalidationTypes.Render);
        }

        private HWND TargetWindowHandle { get; }

        protected override bool IsWindowSubClassed => true;

        private bool IsPositionValid => !InvalidatedValuesHasFlag(FieldInvalidationTypes.Location | FieldInvalidationTypes.Size | FieldInvalidationTypes.Visibility);

        public GlowWindow(Window owner, GlowWindowBehavior behavior, Dock orientation)
        {
            targetWindow = owner ?? throw new ArgumentNullException(nameof(owner));
            this.behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
            this.orientation = orientation;

            TargetWindowHandle = new(new WindowInteropHelper(targetWindow).EnsureHandle());

            if (TargetWindowHandle == IntPtr.Zero
                || PInvoke.IsWindow(TargetWindowHandle) == false)
            {
                throw new Exception($"TargetWindowHandle {TargetWindowHandle} must be a window.");
            }

            title = $"Glow_{this.orientation}";
        }

        private void UpdateProperty<T>(ref T field, T value, FieldInvalidationTypes invalidation)
            where T : struct, IEquatable<T>
        {
            if (field.Equals(value))
            {
                return;
            }

            field = value;
            invalidatedValues |= invalidation;

            if (IsDeferringChanges == false)
            {
                CommitChanges(IntPtr.Zero);
            }
        }

        protected override ushort CreateWindowClassCore()
        {
            return SharedWindowClassAtom;
        }

        protected override void DestroyWindowClassCore()
        {
            // Do nothing here as we registered a shared class/atom
        }

        protected override unsafe IntPtr CreateWindowCore()
        {
            const WINDOW_EX_STYLE EX_STYLE = WINDOW_EX_STYLE.WS_EX_TOOLWINDOW | WINDOW_EX_STYLE.WS_EX_LAYERED;
            const WINDOW_STYLE STYLE = WINDOW_STYLE.WS_POPUP | WINDOW_STYLE.WS_CLIPSIBLINGS | WINDOW_STYLE.WS_CLIPCHILDREN;

            var windowHandle = PInvoke.CreateWindowEx(EX_STYLE, ClassName, title, STYLE, 0, 0, 0, 0, TargetWindowHandle, null, null, null);

            return windowHandle;
        }

        protected override nint WndProc(nint hwnd, uint msg, nuint wParam, nint lParam)
        {
            var message = (WM)msg;
            //System.Diagnostics.Trace.WriteLine($"{DateTime.Now} {hwnd} {message} {wParam} {lParam}");

            switch (message)
            {
                case WM.DESTROY:
                    Dispose();
                    break;

                case WM.NCHITTEST:
                    return (nint)WmNcHitTest(lParam);

                case WM.NCLBUTTONDOWN:
                case WM.NCLBUTTONDBLCLK:
                case WM.NCRBUTTONDOWN:
                case WM.NCRBUTTONDBLCLK:
                case WM.NCMBUTTONDOWN:
                case WM.NCMBUTTONDBLCLK:
                case WM.NCXBUTTONDOWN:
                case WM.NCXBUTTONDBLCLK:
                    {
                        PInvoke.SendMessage(TargetWindowHandle, (uint)message, wParam, IntPtr.Zero);
                        return default;
                    }

                case WM.WINDOWPOSCHANGED:
                case WM.WINDOWPOSCHANGING:
                    {
                        var windowpos = Marshal.PtrToStructure<WINDOWPOS>(lParam);
                        windowpos.flags |= SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE;
                        Marshal.StructureToPtr(windowpos, lParam, true);
                        break;
                    }

                case WM.SETFOCUS:
                    // Move focus back as we don't want to get focused
                    PInvoke.SetFocus(new HWND((nint)wParam));
                    return default;

                case WM.ACTIVATE:
                    return default;

                case WM.NCACTIVATE:
                    PInvoke.SendMessage(TargetWindowHandle, (uint)message, wParam, lParam);
                    // We have to return true according to https://docs.microsoft.com/en-us/windows/win32/winmsg/wm-ncactivate
                    // If we don't do that here the owner window can't be activated.
                    return 1;

                case WM.MOUSEACTIVATE:
                    // WA_CLICKACTIVE = 2
                    PInvoke.SendMessage(TargetWindowHandle, (uint)WM.ACTIVATE, new(2), IntPtr.Zero);

                    return 3 /* MA_NOACTIVATE */;

                case WM.DISPLAYCHANGE:
                    {
                        if (IsVisible)
                        {
                            RenderLayeredWindow();
                        }

                        break;
                    }

                case WM.SHOWWINDOW:
                    {
                        // Prevent glow from getting visible before the owner/parent is visible
                        if (lParam == SW_PARENTOPENING)
                        {
                            return default;
                        }

                        break;
                    }
            }

            return base.WndProc(hwnd, msg, wParam, lParam);
        }

        private unsafe HT WmNcHitTest(IntPtr lParam)
        {
            if (IsDisposed)
            {
                return HT.NOWHERE;
            }

            var xLParam = PInvoke.GetXLParam(lParam.ToInt32());
            var yLParam = PInvoke.GetYLParam(lParam.ToInt32());
            RECT lpRect = default;
            PInvoke.GetWindowRect(Hwnd, &lpRect);

            switch (orientation)
            {
                case Dock.Left:
                    if (yLParam - cornerGripThickness < lpRect.top)
                    {
                        return HT.TOPLEFT;
                    }

                    if (yLParam + cornerGripThickness > lpRect.bottom)
                    {
                        return HT.BOTTOMLEFT;
                    }

                    return HT.LEFT;

                case Dock.Right:
                    if (yLParam - cornerGripThickness < lpRect.top)
                    {
                        return HT.TOPRIGHT;
                    }

                    if (yLParam + cornerGripThickness > lpRect.bottom)
                    {
                        return HT.BOTTOMRIGHT;
                    }

                    return HT.RIGHT;

                case Dock.Top:
                    if (xLParam - cornerGripThickness < lpRect.left)
                    {
                        return HT.TOPLEFT;
                    }

                    if (xLParam + cornerGripThickness > lpRect.right)
                    {
                        return HT.TOPRIGHT;
                    }

                    return HT.TOP;

                default:
                    if (xLParam - cornerGripThickness < lpRect.left)
                    {
                        return HT.BOTTOMLEFT;
                    }

                    if (xLParam + cornerGripThickness > lpRect.right)
                    {
                        return HT.BOTTOMRIGHT;
                    }

                    return HT.BOTTOM;
            }
        }

        public void CommitChanges(IntPtr windowPosInfo)
        {
            InvalidateCachedBitmaps();
            UpdateWindowPosCore(windowPosInfo);
            UpdateLayeredWindowCore();
            invalidatedValues = FieldInvalidationTypes.None;
        }

        private bool InvalidatedValuesHasFlag(FieldInvalidationTypes flag)
        {
            return (invalidatedValues & flag) != 0;
        }

        private void InvalidateCachedBitmaps()
        {
            if (InvalidatedValuesHasFlag(FieldInvalidationTypes.ActiveColor)
                || InvalidatedValuesHasFlag(FieldInvalidationTypes.GlowDepth))
            {
                ClearCache(activeGlowBitmaps);
            }

            if (InvalidatedValuesHasFlag(FieldInvalidationTypes.InactiveColor)
                || InvalidatedValuesHasFlag(FieldInvalidationTypes.GlowDepth))
            {
                ClearCache(inactiveGlowBitmaps);
            }
        }

        private void UpdateWindowPosCore(IntPtr windowPosInfo)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvalidatedValuesHasFlag(FieldInvalidationTypes.Location | FieldInvalidationTypes.Size | FieldInvalidationTypes.Visibility))
            {
                var flags = SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE | SET_WINDOW_POS_FLAGS.SWP_NOOWNERZORDER;
                if (InvalidatedValuesHasFlag(FieldInvalidationTypes.Visibility))
                {
                    flags = IsVisible
                        ? flags | SET_WINDOW_POS_FLAGS.SWP_SHOWWINDOW
                        : flags | SET_WINDOW_POS_FLAGS.SWP_HIDEWINDOW | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOSIZE;
                }

                if (InvalidatedValuesHasFlag(FieldInvalidationTypes.Location) == false)
                {
                    flags |= SET_WINDOW_POS_FLAGS.SWP_NOMOVE;
                }

                if (InvalidatedValuesHasFlag(FieldInvalidationTypes.Size) == false)
                {
                    flags |= SET_WINDOW_POS_FLAGS.SWP_NOSIZE;
                }

                if (windowPosInfo == IntPtr.Zero)
                {
                    PInvoke.SetWindowPos(Hwnd, default, Left, Top, Width, Height, flags);
                }
                else
                {
                    PInvoke.DeferWindowPos(windowPosInfo, Hwnd, default, Left, Top, Width, Height, flags);
                }
            }
        }

        private void UpdateLayeredWindowCore()
        {
            if (IsVisible
                && IsDisposed == false
                && InvalidatedValuesHasFlag(FieldInvalidationTypes.Render))
            {
                if (IsPositionValid)
                {
                    BeginDelayedRender();
                    return;
                }

                CancelDelayedRender();
                RenderLayeredWindow();
            }
        }

        private void BeginDelayedRender()
        {
            if (pendingDelayRender == false)
            {
                pendingDelayRender = true;
                CompositionTarget.Rendering += CommitDelayedRender;
            }
        }

        private void CancelDelayedRender()
        {
            if (pendingDelayRender)
            {
                pendingDelayRender = false;
                CompositionTarget.Rendering -= CommitDelayedRender;
            }
        }

        private void CommitDelayedRender(object? sender, EventArgs e)
        {
            CancelDelayedRender();

            if (IsVisible
                && IsDisposed == false)
            {
                RenderLayeredWindow();
            }
        }

        private unsafe void RenderLayeredWindow()
        {
            if (IsDisposed
                || Width == 0
                || Height == 0)
            {
                return;
            }

            using var glowDrawingContext = new GlowDrawingContext(Width, Height);
            if (glowDrawingContext.IsInitialized == false)
            {
                return;
            }

            switch (orientation)
            {
                case Dock.Left:
                    DrawLeft(glowDrawingContext);
                    break;
                case Dock.Right:
                    DrawRight(glowDrawingContext);
                    break;
                case Dock.Top:
                    DrawTop(glowDrawingContext);
                    break;
                case Dock.Bottom:
                    DrawBottom(glowDrawingContext);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }

            var pptDest = new POINT { x = Left, y = Top };
            var psize = new SIZE { cx = Width, cy = Height };
            var pptSrc = new POINT { x = 0, y = 0 };

            fixed (BLENDFUNCTION* blend = &glowDrawingContext.Blend)
            {
                //PInvoke.UpdateLayeredWindow(this.Hwnd, glowDrawingContext.ScreenDc, pptDest, psize, glowDrawingContext.WindowDc, pptSrc, 0, glowDrawingContext.Blend, UPDATE_LAYERED_WINDOW_FLAGS.ULW_ALPHA);
                PInvoke.UpdateLayeredWindow(Hwnd, new HDC(glowDrawingContext.ScreenDc.DangerousGetHandle()), &pptDest, &psize, new HDC(glowDrawingContext.WindowDc.DangerousGetHandle()), &pptSrc, 0, blend, UPDATE_LAYERED_WINDOW_FLAGS.ULW_ALPHA);
            }
        }

        private GlowBitmap? GetOrCreateBitmap(GlowDrawingContext drawingContext, GlowBitmapPart bitmapPart)
        {
            if (drawingContext.ScreenDc is null)
            {
                return null;
            }

            GlowBitmap?[] array;
            Color color;

            if (IsActive)
            {
                array = activeGlowBitmaps;
                color = ActiveGlowColor;
            }
            else
            {
                array = inactiveGlowBitmaps;
                color = InactiveGlowColor;
            }

            return array[(int)bitmapPart] ?? (array[(int)bitmapPart] = GlowBitmap.Create(drawingContext, bitmapPart, color, GlowDepth, UseRadialGradientForCorners));
        }

        private static void ClearCache(GlowBitmap?[] cache)
        {
            for (var i = 0; i < cache.Length; i++)
            {
                using (cache[i])
                {
                    cache[i] = null;
                }
            }
        }

        protected override void DisposeManagedResources()
        {
            ClearCache(activeGlowBitmaps);
            ClearCache(inactiveGlowBitmaps);
        }

        private void DrawLeft(GlowDrawingContext drawingContext)
        {
            if (drawingContext.ScreenDc is null
                || drawingContext.WindowDc is null
                || drawingContext.BackgroundDc is null)
            {
                return;
            }

            var cornerTopLeftBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerTopLeft)!;
            var leftTopBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.LeftTop)!;
            var leftBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.Left)!;
            var leftBottomBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.LeftBottom)!;
            var cornerBottomLeftBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerBottomLeft)!;

            var bitmapHeight = cornerTopLeftBitmap.Height;
            var num = bitmapHeight + leftTopBitmap.Height;
            var num2 = drawingContext.Height - cornerBottomLeftBitmap.Height;
            var num3 = num2 - leftBottomBitmap.Height;
            var num4 = num3 - num;

            PInvoke.SelectObject(drawingContext.BackgroundDc, cornerTopLeftBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, 0, 0, cornerTopLeftBitmap.Width, cornerTopLeftBitmap.Height, drawingContext.BackgroundDc, 0, 0, cornerTopLeftBitmap.Width, cornerTopLeftBitmap.Height, drawingContext.Blend);
            PInvoke.SelectObject(drawingContext.BackgroundDc, leftTopBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, 0, bitmapHeight, leftTopBitmap.Width, leftTopBitmap.Height, drawingContext.BackgroundDc, 0, 0, leftTopBitmap.Width, leftTopBitmap.Height, drawingContext.Blend);

            if (num4 > 0)
            {
                PInvoke.SelectObject(drawingContext.BackgroundDc, leftBitmap.Handle);
                PInvoke.AlphaBlend(drawingContext.WindowDc, 0, num, leftBitmap.Width, num4, drawingContext.BackgroundDc, 0, 0, leftBitmap.Width, leftBitmap.Height, drawingContext.Blend);
            }

            PInvoke.SelectObject(drawingContext.BackgroundDc, leftBottomBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, 0, num3, leftBottomBitmap.Width, leftBottomBitmap.Height, drawingContext.BackgroundDc, 0, 0, leftBottomBitmap.Width, leftBottomBitmap.Height, drawingContext.Blend);
            PInvoke.SelectObject(drawingContext.BackgroundDc, cornerBottomLeftBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, 0, num2, cornerBottomLeftBitmap.Width, cornerBottomLeftBitmap.Height, drawingContext.BackgroundDc, 0, 0, cornerBottomLeftBitmap.Width, cornerBottomLeftBitmap.Height, drawingContext.Blend);
        }

        private void DrawRight(GlowDrawingContext drawingContext)
        {
            if (drawingContext.ScreenDc is null
                || drawingContext.WindowDc is null
                || drawingContext.BackgroundDc is null)
            {
                return;
            }

            var cornerTopRightBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerTopRight)!;
            var rightTopBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.RightTop)!;
            var rightBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.Right)!;
            var rightBottomBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.RightBottom)!;
            var cornerBottomRightBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerBottomRight)!;

            var bitmapHeight = cornerTopRightBitmap.Height;
            var num = bitmapHeight + rightTopBitmap.Height;
            var num2 = drawingContext.Height - cornerBottomRightBitmap.Height;
            var num3 = num2 - rightBottomBitmap.Height;
            var num4 = num3 - num;

            PInvoke.SelectObject(drawingContext.BackgroundDc, cornerTopRightBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, 0, 0, cornerTopRightBitmap.Width, cornerTopRightBitmap.Height, drawingContext.BackgroundDc, 0, 0, cornerTopRightBitmap.Width, cornerTopRightBitmap.Height, drawingContext.Blend);
            PInvoke.SelectObject(drawingContext.BackgroundDc, rightTopBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, 0, bitmapHeight, rightTopBitmap.Width, rightTopBitmap.Height, drawingContext.BackgroundDc, 0, 0, rightTopBitmap.Width, rightTopBitmap.Height, drawingContext.Blend);

            if (num4 > 0)
            {
                PInvoke.SelectObject(drawingContext.BackgroundDc, rightBitmap.Handle);
                PInvoke.AlphaBlend(drawingContext.WindowDc, 0, num, rightBitmap.Width, num4, drawingContext.BackgroundDc, 0, 0, rightBitmap.Width, rightBitmap.Height, drawingContext.Blend);
            }

            PInvoke.SelectObject(drawingContext.BackgroundDc, rightBottomBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, 0, num3, rightBottomBitmap.Width, rightBottomBitmap.Height, drawingContext.BackgroundDc, 0, 0, rightBottomBitmap.Width, rightBottomBitmap.Height, drawingContext.Blend);
            PInvoke.SelectObject(drawingContext.BackgroundDc, cornerBottomRightBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, 0, num2, cornerBottomRightBitmap.Width, cornerBottomRightBitmap.Height, drawingContext.BackgroundDc, 0, 0, cornerBottomRightBitmap.Width, cornerBottomRightBitmap.Height, drawingContext.Blend);
        }

        private void DrawTop(GlowDrawingContext drawingContext)
        {
            if (drawingContext.ScreenDc is null
                || drawingContext.WindowDc is null
                || drawingContext.BackgroundDc is null)
            {
                return;
            }

            var topLeftBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.TopLeft)!;
            var topBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.Top)!;
            var topRightBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.TopRight)!;

            var num = GlowDepth;
            var num2 = num + topLeftBitmap.Width;
            var num3 = drawingContext.Width - GlowDepth - topRightBitmap.Width;
            var num4 = num3 - num2;

            PInvoke.SelectObject(drawingContext.BackgroundDc, topLeftBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, num, 0, topLeftBitmap.Width, topLeftBitmap.Height, drawingContext.BackgroundDc, 0, 0, topLeftBitmap.Width, topLeftBitmap.Height, drawingContext.Blend);

            if (num4 > 0)
            {
                PInvoke.SelectObject(drawingContext.BackgroundDc, topBitmap.Handle);
                PInvoke.AlphaBlend(drawingContext.WindowDc, num2, 0, num4, topBitmap.Height, drawingContext.BackgroundDc, 0, 0, topBitmap.Width, topBitmap.Height, drawingContext.Blend);
            }

            PInvoke.SelectObject(drawingContext.BackgroundDc, topRightBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, num3, 0, topRightBitmap.Width, topRightBitmap.Height, drawingContext.BackgroundDc, 0, 0, topRightBitmap.Width, topRightBitmap.Height, drawingContext.Blend);
        }

        private void DrawBottom(GlowDrawingContext drawingContext)
        {
            if (drawingContext.ScreenDc is null
                || drawingContext.WindowDc is null
                || drawingContext.BackgroundDc is null)
            {
                return;
            }

            var bottomLeftBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.BottomLeft)!;
            var bottomBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.Bottom)!;
            var bottomRightBitmap = GetOrCreateBitmap(drawingContext, GlowBitmapPart.BottomRight)!;

            var num = GlowDepth;
            var num2 = num + bottomLeftBitmap.Width;
            var num3 = drawingContext.Width - GlowDepth - bottomRightBitmap.Width;
            var num4 = num3 - num2;

            PInvoke.SelectObject(drawingContext.BackgroundDc, bottomLeftBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, num, 0, bottomLeftBitmap.Width, bottomLeftBitmap.Height, drawingContext.BackgroundDc, 0, 0, bottomLeftBitmap.Width, bottomLeftBitmap.Height, drawingContext.Blend);

            if (num4 > 0)
            {
                PInvoke.SelectObject(drawingContext.BackgroundDc, bottomBitmap.Handle);
                PInvoke.AlphaBlend(drawingContext.WindowDc, num2, 0, num4, bottomBitmap.Height, drawingContext.BackgroundDc, 0, 0, bottomBitmap.Width, bottomBitmap.Height, drawingContext.Blend);
            }

            PInvoke.SelectObject(drawingContext.BackgroundDc, bottomRightBitmap.Handle);
            PInvoke.AlphaBlend(drawingContext.WindowDc, num3, 0, bottomRightBitmap.Width, bottomRightBitmap.Height, drawingContext.BackgroundDc, 0, 0, bottomRightBitmap.Width, bottomRightBitmap.Height, drawingContext.Blend);
        }

        public void UpdateWindowPos()
        {
            var targetWindowHandle = TargetWindowHandle;

            if (IsVisible == false
                || PInvoke.GetMappedClientRect(targetWindowHandle, out var lpRect) == false)
            {
                return;
            }

            switch (orientation)
            {
                case Dock.Left:
                    Left = lpRect.left - GlowDepth;
                    Top = lpRect.top - GlowDepth;
                    Width = GlowDepth;
                    Height = lpRect.GetHeight() + GlowDepth + GlowDepth;
                    break;

                case Dock.Top:
                    Left = lpRect.left - GlowDepth;
                    Top = lpRect.top - GlowDepth;
                    Width = lpRect.GetWidth() + GlowDepth + GlowDepth;
                    Height = GlowDepth;
                    break;

                case Dock.Right:
                    Left = lpRect.right;
                    Top = lpRect.top - GlowDepth;
                    Width = GlowDepth;
                    Height = lpRect.GetHeight() + GlowDepth + GlowDepth;
                    break;

                case Dock.Bottom:
                    Left = lpRect.left - GlowDepth;
                    Top = lpRect.bottom;
                    Width = lpRect.GetWidth() + GlowDepth + GlowDepth;
                    Height = GlowDepth;
                    break;
            }
        }
    }
}