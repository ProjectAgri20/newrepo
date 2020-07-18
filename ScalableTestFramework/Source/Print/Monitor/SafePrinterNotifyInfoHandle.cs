using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// <see cref="SafeHandle" /> for a printer notification info object.
    /// </summary>
    internal sealed class SafePrinterNotifyInfoHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafePrinterNotifyInfoHandle" /> class.
        /// </summary>
        public SafePrinterNotifyInfoHandle()
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
                NativeMethods.FreePrinterNotifyInfo(handle);
                handle = IntPtr.Zero;
                return true;
            }
            return false;
        }

        private static class NativeMethods
        {
            [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool FreePrinterNotifyInfo(IntPtr pPropertyNotifyInfo);
        }
    }
}
