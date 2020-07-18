using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace HP.ScalableTest.WindowsAutomation
{
    /// <summary>
    /// <see cref="SafeHandle" /> for unmanaged memory created via <see cref="Marshal.AllocHGlobal(int)" />.
    /// </summary>
    public sealed class SafeUnmanagedMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeUnmanagedMemoryHandle" /> class.
        /// </summary>
        /// <param name="length">The number of bytes to allocate.</param>
        public SafeUnmanagedMemoryHandle(int length)
            : base(true)
        {
            SetHandle(Marshal.AllocHGlobal(length));
        }

        /// <summary>
        /// When overridden in a derived class, executes the code required to free the handle.
        /// </summary>
        /// <returns><c>true</c> if the release was successful, <c>false</c> otherwise.</returns>
        protected override bool ReleaseHandle()
        {
            if (handle != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(handle);
                handle = IntPtr.Zero;
                return true;
            }
            return false;
        }
    }
}
