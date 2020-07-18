using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// <para>The Windows API (Win32) has the FlashWindowEx method within the User32 library; this
    /// method allows a developer to Flash a Window, signifying to the user that some major event
    /// occurred within the application that requires their attention. The most common use of this
    /// is to flash the window until the user returns focus to the application. However, you can
    /// also flash the window a specified number of times, or just keep flashing it until you
    /// decide when to stop.</para>
    /// 
    /// <para>The code in this class was leveraged from the web site:
    /// http://pietschsoft.com/post/2009/01/26/CSharp-Flash-Window-in-Taskbar-via-Win32-FlashWindowEx
    /// </para>
    /// </summary>
    public static class FlashWindow
    {
        /// <summary>
        /// Declare the interface to a method in the User32.dll library that will flash the
        /// specified window.
        /// </summary>
        /// <param name="pwfi">A pointer to a <see cref="FLASHWINFO"/> structure.</param>
        /// <returns>The return value specifies the window's state before the call to the
        /// <b>FlashWindowEx</b> function. If the window caption was drawn as active before the
        /// call, the return value is true. Otherwise, the return value is false.</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        /// <summary>
        /// Contains the flash status for a window and the number of times the system should flash
        /// the window.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            /// <summary>
            /// The size of the structure, in bytes.
            /// </summary>
            public uint cbSize;
            /// <summary>
            /// A handle to the window to be flashed. The window can be either opened or minimized.
            /// </summary>
            public IntPtr hwnd;
            /// <summary>
            /// The flash status.
            /// </summary>
            public uint dwFlags;
            /// <summary>
            /// The number of times to flash the window.
            /// </summary>
            public uint uCount;
            /// <summary>
            /// The rate at which the window is to be flashed, in milliseconds. If <b>dwTimeout</b>
            /// is zero, the function uses the default cursor blink rate.
            /// </summary>
            public uint dwTimeout;
        }

        /// <summary>
        /// Stop flashing. The system restores the window to its original state.
        /// </summary>
        public const uint FLASHW_STOP = 0;

        /// <summary>
        /// Flash the window caption.
        /// </summary>
        public const uint FLASHW_CAPTION = 1;

        /// <summary>
        /// Flash the taskbar button.
        /// </summary>
        public const uint FLASHW_TRAY = 2;

        /// <summary>
        /// Flash both the window caption and taskbar button. This is equivalent to setting the
        /// FLASHW_CAPTION | FLASHW_TRAY flags.
        /// </summary>
        public const uint FLASHW_ALL = 3;

        /// <summary>
        /// Flash continuously, until the FLASHW_STOP flag is set.
        /// </summary>
        public const uint FLASHW_TIMER = 4;

        /// <summary>
        /// Flash continuously until the window comes to the foreground.
        /// </summary>
        public const uint FLASHW_TIMERNOFG = 12;

        /// <summary>
        /// Flash the specified Window (Form) until it receives focus.
        /// </summary>
        /// <param name="form">The Form (Window) to Flash.</param>
        /// <returns></returns>
        public static bool Flash(Form form)
        {
            FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL | FLASHW_TIMERNOFG, uint.MaxValue, 0);
            return FlashWindowEx(ref fi);
        }

        /// <summary>
        /// Flash the specified Window (form) for the specified number of times.
        /// </summary>
        /// <param name="form">The Form (Window) to Flash.</param>
        /// <param name="count">The number of times to Flash.</param>
        /// <returns></returns>
        public static bool Flash(Form form, uint count)
        {
            FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL, count, 0);
            return FlashWindowEx(ref fi);
        }

        /// <summary>
        /// Start Flashing the specified Window (form).
        /// </summary>
        /// <param name="form">The Form (Window) to Flash.</param>
        /// <returns></returns>
        public static bool Start(Form form)
        {
            FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL, uint.MaxValue, 0);
            return FlashWindowEx(ref fi);
        }

        /// <summary>
        /// Stop Flashing the specified Window (form).
        /// </summary>
        /// <param name="form">The Form (Window) to stop Flashing.</param>
        /// <returns></returns>
        public static bool Stop(Form form)
        {
            FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_STOP, uint.MaxValue, 0);
            return FlashWindowEx(ref fi);
        }

        private static FLASHWINFO Create_FLASHWINFO(IntPtr handle, uint flags, uint count, uint timeout)
        {
            FLASHWINFO fi = new FLASHWINFO();
            fi.cbSize = Convert.ToUInt32(Marshal.SizeOf(fi));
            fi.hwnd = handle;
            fi.dwFlags = flags;
            fi.uCount = count;
            fi.dwTimeout = timeout;
            return fi;
        }
    }
}
