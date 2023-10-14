using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Hearthstone_Deck_Tracker;
using Gma.System.MouseKeyHook;

namespace HDT_BGhelper
{
	internal class BGhelper
	{
        const int MOUSEEVENTF_LEFTDOWN = 0x02;
        const int MOUSEEVENTF_LEFTUP = 0x04;
        private IKeyboardMouseEvents m_GlobalHook;

        [DllImport("user32.dll", SetLastError = true)]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        static extern void SetCursorPos(int x, int y);

		internal void GameStart()
		{
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
        }

        internal void InMenu()
		{
            Disable();
        }

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (Core.Overlay.IsVisible)
            {
                if (e.Button == MouseButtons.Right)
                {
                    double x = Core.Overlay.Left;
                    double y = Core.Overlay.Top;

                    var size = Core.Overlay.RenderSize;
                    double w = size.Width;
                    double h = size.Height;

                    double dx = w / 1920 * 1130;
                    double dy = h / 1080 * 200;

                    double nx = w / 1920 * 960;
                    double ny = h / 1080 * 700;

                    SetCursorPos(Convert.ToInt32(x + dx), Convert.ToInt32(y + dy));
                    System.Threading.Thread.Sleep(10);
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                    System.Threading.Thread.Sleep(10);
                    SetCursorPos(Convert.ToInt32(x + nx), Convert.ToInt32(y + ny));
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    double x = Core.Overlay.Left;
                    double y = Core.Overlay.Top;

                    var size = Core.Overlay.RenderSize;
                    double w = size.Width;
                    double h = size.Height;

                    double dx = w / 1920 * 1240;
                    double dy = h / 1080 * 170;

                    double nx = w / 1920 * 960;
                    double ny = h / 1080 * 700;

                    SetCursorPos(Convert.ToInt32(x + dx), Convert.ToInt32(y + dy));
                    System.Threading.Thread.Sleep(10);
                    mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                    System.Threading.Thread.Sleep(10);
                    SetCursorPos(Convert.ToInt32(x + nx), Convert.ToInt32(y + ny));
                }
            }
        }

        public void Disable()
        {
            m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
            m_GlobalHook.Dispose();
        }
	}
}