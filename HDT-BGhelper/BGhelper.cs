using System;
using System.Runtime.InteropServices;
using Hearthstone_Deck_Tracker;
using static Hearthstone_Deck_Tracker.User32;

namespace HDT_BGhelper
{
    internal class BGhelper
    {
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const double rx = 1130.0 / 1920.0;
        private const double ry = 200.0 / 1080.0;
        private const double fx = 1240.0 / 1920.0;
        private const double fy = 170.0 / 1080.0;

        private MouseHook hsHook = null;

        [DllImport("user32.dll")]
        private static extern void SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private static IntPtr CreateLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }

        internal void Activate()
        {
            if (hsHook == null)
            {
                hsHook = new MouseHook();
                hsHook.RmbDown += HsHook_RmbDown;
                hsHook.MmbDown += HsHook_MmbDown;
            }
        }

        internal void Deactivate()
        {
            if (hsHook != null)
            {
                hsHook.Dispose();
                hsHook = null;
            }
        }

        private void HsHook_RmbDown(object sender, EventArgs e)
        {
            if (IsHearthstoneInForeground())
            {
                int x = (int)(Core.Overlay.Left);
                int y = (int)(Core.Overlay.Top);
                int dx = (int)(Core.Overlay.RenderSize.Width * rx);
                int dy = (int)(Core.Overlay.RenderSize.Height * ry);
                var handle = GetHearthstoneWindow();
                var lparam = CreateLParam(dx, dy);
                var oripos = GetMousePos();

                SetCursorPos(x + dx, y + dy);
                SendMessage(handle, WM_LBUTTONDOWN, IntPtr.Zero, lparam);
                SendMessage(handle, WM_LBUTTONUP, IntPtr.Zero, lparam);
                SetCursorPos(oripos.X, oripos.Y);
            }
        }

        private void HsHook_MmbDown(object sender, EventArgs e)
        {
            if (IsHearthstoneInForeground())
            {
                int x = (int)(Core.Overlay.Left);
                int y = (int)(Core.Overlay.Top);
                int dx = (int)(Core.Overlay.RenderSize.Width * fx);
                int dy = (int)(Core.Overlay.RenderSize.Height * fy);
                var handle = GetHearthstoneWindow();
                var lparam = CreateLParam(dx, dy);
                var oripos = GetMousePos();

                SetCursorPos(x + dx, y + dy);
                SendMessage(handle, WM_LBUTTONDOWN, IntPtr.Zero, lparam);
                SendMessage(handle, WM_LBUTTONUP, IntPtr.Zero, lparam);
                SetCursorPos(oripos.X, oripos.Y);
            }
        }

        // http://joelabrahamsson.com/detecting-mouse-and-keyboard-input-with-net/
        private class MouseHook : IDisposable
        {
            private const int WH_MOUSE_LL = 14;
            private const int WM_RBUTTONDOWN = 0x0204;
            private const int WM_MBUTTONDOWN = 0x0207;
            private readonly WindowsHookHelper.HookDelegate _mouseDelegate;
            private readonly IntPtr _mouseHandle;
            private bool _disposed;

            public MouseHook()
            {
                _mouseDelegate = MouseHookDelegate; // crashes application if directly used for some reason
                _mouseHandle = WindowsHookHelper.SetWindowsHookEx(WH_MOUSE_LL, _mouseDelegate, IntPtr.Zero, 0);
            }

            public void Dispose() => Dispose(true);

            #nullable enable
            public event EventHandler<EventArgs>? RmbDown;
            public event EventHandler<EventArgs>? MmbDown;
            #nullable disable

            private IntPtr MouseHookDelegate(int code, IntPtr wParam, IntPtr lParam)
            {
                if (code < 0)
                    return WindowsHookHelper.CallNextHookEx(_mouseHandle, code, wParam, lParam);

                switch (wParam.ToInt32())
                {
                    case WM_RBUTTONDOWN:
                        RmbDown?.Invoke(this, new EventArgs());
                        break;
                    case WM_MBUTTONDOWN:
                        MmbDown?.Invoke(this, new EventArgs());
                        break;
                }

                return WindowsHookHelper.CallNextHookEx(_mouseHandle, code, wParam, lParam);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                    return;
                if (_mouseHandle != IntPtr.Zero)
                    WindowsHookHelper.UnhookWindowsHookEx(_mouseHandle);
                _disposed = true;
            }

            ~MouseHook()
            {
                Dispose(false);
            }
        }
    }
}