using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Raised Panel
    /// </summary>
    public class RaisedPanel : Panel
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_DLGMODALFRAME = 0x00000001;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int SWP_FRAMECHANGED = 0x0020;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        /// <summary>
        /// Constructor
        /// </summary>
        public RaisedPanel()
        {
            uint options = (SWP_FRAMECHANGED | SWP_NOSIZE | SWP_NOMOVE | SWP_NOZORDER | SWP_NOACTIVATE);

            int topStyle = GetWindowLong(Handle, GWL_EXSTYLE);
            SetWindowLong(Handle, GWL_EXSTYLE, (topStyle | WS_EX_DLGMODALFRAME));
            SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0, options);
        }
    }
}
