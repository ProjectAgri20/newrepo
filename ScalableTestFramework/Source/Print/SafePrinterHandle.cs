using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// <see cref="SafeHandle" /> for native printer handles.
    /// </summary>
    internal sealed class SafePrinterHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafePrinterHandle" /> class.
        /// </summary>
        public SafePrinterHandle()
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
                NativeMethods.ClosePrinter(handle);
                handle = IntPtr.Zero;
                return true;
            }
            return false;
        }

        private static class NativeMethods
        {
            [DllImport("winspool.drv", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool ClosePrinter(IntPtr hPrinter);
        }
    }
}
