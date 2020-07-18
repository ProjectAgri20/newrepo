using System;
using System.Runtime.InteropServices;

namespace HP.ScalableTest.WindowsAutomation.Sspi
{
    /// <summary>
    /// A safe encapsulation for a native handle used in the SSPI API.
    /// </summary>
    internal abstract class SafeSspiHandle : SafeHandle
    {
        /// <summary>
        /// Gets the <see cref="RawSspiHandle" /> wrapped by this instance.
        /// </summary>
        internal RawSspiHandle RawHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeSspiHandle" /> class.
        /// </summary>
        protected SafeSspiHandle()
            : base(IntPtr.Zero, true)
        {
            RawHandle = new RawSspiHandle();
        }

        /// <summary>
        /// Gets a value indicating whether the handle value is invalid.
        /// </summary>
        public override bool IsInvalid
        {
            get { return IsClosed || RawHandle.IsZero; }
        }

        /// <summary>
        /// Executes the code required to free the handle.
        /// </summary>
        /// <returns>true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false.</returns>
        protected override bool ReleaseHandle()
        {
            RawHandle.SetInvalid();
            return true;
        }

        /// <summary>
        /// Represents the raw structure for any handle created for the SSPI API.
        /// Any SSPI handle is always the size of two native pointers.
        /// This class should never be referenced directly - only through <see cref="SafeSspiHandle" />.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct RawSspiHandle
        {
            private IntPtr _lower;
            private IntPtr _upper;

            /// <summary>
            /// Gets a value indicating whether this instance is set to the default, empty value.
            /// </summary>
            public bool IsZero
            {
                get { return _lower == IntPtr.Zero && _upper == IntPtr.Zero; }
            }

            /// <summary>
            /// Sets the handle to an invalid (zero) value.
            /// </summary>
            public void SetInvalid()
            {
                _lower = IntPtr.Zero;
                _upper = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Represents a Windows API Timestamp structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        protected struct Timestamp
        {
            private long _time;
        }
    }
}
