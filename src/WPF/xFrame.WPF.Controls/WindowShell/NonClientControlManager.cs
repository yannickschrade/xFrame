//Code from controlzEx library https://github.com/ControlzEx/ControlzEx
using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Interop;
using System.Windows.Media;
using Windows.Win32;
using xFrame.WPF.Controls.Internal;
using xFrame.WPF.Controls.Native;

namespace xFrame.WPF.Controls.WindowShell
{
    public class NonClientControlManager
    {
        private DependencyObject trackedControl;

        public NonClientControlManager(Window window)
        {
            Owner = window ?? throw new ArgumentNullException(nameof(window));
            OwnerHandle = new WindowInteropHelper(Owner).Handle;
        }

        private Window Owner { get; }

        private IntPtr OwnerHandle { get; }

        public void ClearTrackedControl()
        {
            if (trackedControl is null)
            {
                return;
            }

            NonClientControlProperties.SetIsNCMouseOver(trackedControl, false);
            NonClientControlProperties.SetIsNCPressed(trackedControl, false);
            trackedControl = null;
        }

        public bool HoverTrackedControl(IntPtr lParam)
        {
            var controlUnderMouse = GetControlUnderMouse(lParam);
            if (controlUnderMouse == trackedControl)
            {
                return true;
            }

            if (trackedControl is not null)
            {
                NonClientControlProperties.SetIsNCMouseOver(trackedControl, false);
                NonClientControlProperties.SetIsNCPressed(trackedControl, false);
            }

            trackedControl = controlUnderMouse;

            if (trackedControl is not null)
            {
                NonClientControlProperties.SetIsNCMouseOver(trackedControl, true);
            }

            return true;
        }

        public bool PressTrackedControl(IntPtr lParam)
        {
            var controlUnderMouse = GetControlUnderMouse(lParam);
            if (controlUnderMouse != trackedControl)
            {
                HoverTrackedControl(lParam);
            }

            if (trackedControl is null)
            {
                return false;
            }

            NonClientControlProperties.SetIsNCPressed(trackedControl, true);

            var nonClientControlClickStrategy = NonClientControlProperties.GetClickStrategy(trackedControl);
            if (nonClientControlClickStrategy is NonClientControlClickStrategy.MouseEvent)
            {
                // Raising LBUTTONDOWN here automatically causes a LBUTTONUP to be raised by windows later correctly
                PInvoke.RaiseMouseMessage(OwnerHandle, WM.LBUTTONDOWN, default, lParam);

                return true;
            }

            return nonClientControlClickStrategy != NonClientControlClickStrategy.None;
        }

        public bool ClickTrackedControl(IntPtr lParam)
        {
            var controlUnderMouse = GetControlUnderMouse(lParam);
            if (controlUnderMouse != trackedControl)
            {
                return false;
            }

            if (trackedControl is null)
            {
                return false;
            }

            if (NonClientControlProperties.GetIsNCPressed(trackedControl) == false)
            {
                return false;
            }

            NonClientControlProperties.SetIsNCPressed(trackedControl, false);

            if (NonClientControlProperties.GetClickStrategy(trackedControl) is NonClientControlClickStrategy.AutomationPeer
                && trackedControl is UIElement uiElement
                && UIElementAutomationPeer.CreatePeerForElement(uiElement).GetPattern(PatternInterface.Invoke) is IInvokeProvider invokeProvider)
            {
                invokeProvider.Invoke();
            }

            return false;
        }

        public DependencyObject GetControlUnderMouse(IntPtr lParam)
        {
            return GetControlUnderMouse(Owner, lParam);
        }

        public static DependencyObject GetControlUnderMouse(Window owner, IntPtr lParam)
        {
            return GetControlUnderMouse(owner, lParam, out _);
        }

        public static DependencyObject GetControlUnderMouse(Window owner, IntPtr lParam, out HT hitTestResult)
        {
            if (lParam == IntPtr.Zero)
            {
                hitTestResult = HT.NOWHERE;
                return null;
            }

            var point = LogicalPointFromLParam(owner, lParam);

            if (owner.InputHitTest(point) is Visual visualHit
                && NonClientControlProperties.GetHitTestResult(visualHit) is var res
                && res != HT.NOWHERE)
            {
                hitTestResult = res;

                // If the cursor is on the window edge we must not hit test controls.
                // Otherwise we have no chance to un-track controls when the cursor leaves the window.
                // todo: find a better solution as this does not completely solve the problem when the mouse is moved over the buttons fast and then leaves the window...
                if (hitTestResult is HT.MAXBUTTON or HT.MINBUTTON or HT.CLOSE)
                {
                    if (point.X.AreClose(0)
                        || point.X.AreClose(owner.Width)
                        || point.Y.AreClose(0)
                        || point.Y.AreClose(owner.Height))
                    {
                        hitTestResult = HT.NOWHERE;
                        return null;
                    }
                }

                DependencyObject control = visualHit;
                var currentControl = control;

                while (currentControl is not null)
                {
                    var valueSource = DependencyPropertyHelper.GetValueSource(currentControl, NonClientControlProperties.HitTestResultProperty);
                    if (valueSource.BaseValueSource is not BaseValueSource.Inherited and not BaseValueSource.Unknown)
                    {
                        control = currentControl;
                        break;
                    }

                    currentControl = GetVisualOrLogicalParent(currentControl);
                }

                return control;
            }

            hitTestResult = HT.NOWHERE;
            return null;

            static Point LogicalPointFromLParam(Window owner, IntPtr lParam)
            {
                var point2 = Utility.GetPoint(lParam);
                return owner.PointFromScreen(point2);
            }
        }

        private static DependencyObject GetVisualOrLogicalParent(DependencyObject sourceElement)
        {
            return sourceElement switch
            {
                null => null,
                Visual => VisualTreeHelper.GetParent(sourceElement) ?? LogicalTreeHelper.GetParent(sourceElement),
                _ => LogicalTreeHelper.GetParent(sourceElement)
            };
        }
    }
}