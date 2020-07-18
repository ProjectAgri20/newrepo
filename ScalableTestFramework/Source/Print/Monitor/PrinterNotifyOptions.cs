using System;
using System.Runtime.InteropServices;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Specifies options for a change notification object that monitors a printer or print server.
    /// </summary>
    /// <remarks>
    /// Defined in WinSpool.h
    /// typedef struct _PRINTER_NOTIFY_OPTIONS {
    ///     DWORD Version;
    ///     DWORD Flags;
    ///     DWORD Count;
    ///     PPRINTER_NOTIFY_OPTIONS_TYPE pTypes;
    /// } PRINTER_NOTIFY_OPTIONS, *PPRINTER_NOTIFY_OPTIONS, *LPPRINTER_NOTIFY_OPTIONS;
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct PrinterNotifyOptions : IDisposable
    {
        private uint _version;
        private uint _flags;
        private uint _count;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources", Justification = ".NET doesn't support marshalling from/to a SafeHandle.  This field is required for the Win32 API.")]
        private IntPtr _pTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterNotifyOptions" /> struct.
        /// </summary>
        /// <param name="types">The collection of <see cref="PrinterNotifyOptionsType" /> to subscribe to.</param>
        public PrinterNotifyOptions(params PrinterNotifyOptionsType[] types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            if (types.Length < 1)
            {
                throw new ArgumentException("Types must contain at least one element.", nameof(types));
            }

            _version = 2;
            _flags = 1;
            _count = (uint)types.Length;
            int size = Marshal.SizeOf<PrinterNotifyOptionsType>();
            _pTypes = Marshal.AllocCoTaskMem(size * types.Length);
            long ptr = _pTypes.ToInt64();
            foreach (PrinterNotifyOptionsType type in types)
            {
                Marshal.StructureToPtr(type, new IntPtr(ptr), false);
                ptr += size;
            }
        }

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_pTypes != IntPtr.Zero)
            {
                // pTypes likely has pointers that also need to be freed.
                int size = Marshal.SizeOf<PrinterNotifyOptionsType>();
                long ptr = _pTypes.ToInt64();
                for (int i = 0; i < _count; i++)
                {
                    PrinterNotifyOptionsType type = Marshal.PtrToStructure<PrinterNotifyOptionsType>(new IntPtr(ptr));
                    type.Dispose();
                    ptr += size;
                }

                Marshal.FreeCoTaskMem(_pTypes);
                _pTypes = IntPtr.Zero;
            }
        }
    }
}
