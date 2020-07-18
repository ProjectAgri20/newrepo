using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// <see cref="SafeHandle" /> for a printer change notification object.
    /// </summary>
    internal sealed class SafePrinterChangeNotificationHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafePrinterChangeNotificationHandle" /> class.
        /// </summary>
        public SafePrinterChangeNotificationHandle()
            : base(true)
        {
        }

        /// <summary>
        /// When overridden in a derived class, executes the code required to free the handle.
        /// </summary>
        /// <returns><c>true</c> if the release was successful, <c>false</c> otherwise.</returns>
        protected override bool ReleaseHandle()
        {
            if (handle != IntPtr.Zero)
            {
                NativeMethods.FindClosePrinterChangeNotification(handle);
                handle = IntPtr.Zero;
                return true;
            }
            return false;
        }

        private static class NativeMethods
        {
            [DllImport("winspool.drv", CharSet = CharSet.Auto, EntryPoint = "FindClosePrinterChangeNotification", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FindClosePrinterChangeNotification(IntPtr hChange);
        }
    }
}
