using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Hearthstone_Deck_Tracker;
using Gma.System.MouseKeyHook;

namespace HDT_BGhelper
{
	internal class BGhelper
	{
        private bool hookExist;
        private IKeyboardMouseEvents m_GlobalHook;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        
        public BGhelper()
        {
            hookExist = false;
            m_GlobalHook = Hook.GlobalEvents();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr WindowFromPoint(Point point);

        [DllImport("user32.dll")]
        static extern void SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out Point point);

        private static IntPtr CreateLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }

        internal void GameStart()
		{
            if (!hookExist)
            {
                m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
                hookExist = true;
            }
        }

        internal void GameEnd()
        {
            Disable();
        }

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (Core.Overlay.IsVisible)
            {
                if (e.Button == MouseButtons.Right)
                {
                    int x = (int)(Core.Overlay.Left);
                    int y = (int)(Core.Overlay.Top);
                    int dx = (int)(Core.Overlay.RenderSize.Width / 1920 * 1130);
                    int dy = (int)(Core.Overlay.RenderSize.Height / 1080 * 200);
                    var handle = WindowFromPoint(new Point(x, y));
                    var lparam = CreateLParam(dx, dy);

                    GetCursorPos(out Point oripos);
                    SetCursorPos(x + dx, y + dy);
                    SendMessage(handle, WM_LBUTTONDOWN, IntPtr.Zero, lparam);
                    SendMessage(handle, WM_LBUTTONUP, IntPtr.Zero, lparam);
                    SetCursorPos(oripos.X, oripos.Y);
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    int x = (int)(Core.Overlay.Left);
                    int y = (int)(Core.Overlay.Top);
                    int dx = (int)(Core.Overlay.RenderSize.Width / 1920 * 1240);
                    int dy = (int)(Core.Overlay.RenderSize.Height / 1080 * 170);
                    var handle = WindowFromPoint(new Point(x, y));
                    var lparam = CreateLParam(dx, dy);

                    GetCursorPos(out Point oripos);
                    SetCursorPos(x + dx, y + dy);
                    SendMessage(handle, WM_LBUTTONDOWN, IntPtr.Zero, lparam);
                    SendMessage(handle, WM_LBUTTONUP, IntPtr.Zero, lparam);
                    SetCursorPos(oripos.X, oripos.Y);
                }
            }
        }

        public void Disable()
        {
            if (hookExist)
            {
                m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
                hookExist = false;
            }
        }
	}
}