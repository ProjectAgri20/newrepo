using System;
using System.Runtime.InteropServices;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Specifies the set of printer or job information fields to be monitored by a printer change notification object.
    /// </summary>
    /// <remarks>
    /// Defined in WinSpool.h
    /// typedef struct _PRINTER_NOTIFY_OPTIONS_TYPE {
    ///     WORD Type;
    ///     WORD Reserved0;
    ///     DWORD Reserved1;
    ///     DWORD Reserved2;
    ///     DWORD Count;
    ///     PWORD pFields;
    /// } PRINTER_NOTIFY_OPTIONS_TYPE, *PPRINTER_NOTIFY_OPTIONS_TYPE, *LPPRINTER_NOTIFY_OPTIONS_TYPE;
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct PrinterNotifyOptionsType : IDisposable
    {
        private ushort _type;
        private ushort _reserved0;
        private uint _reserved1;
        private uint _reserved2;
        private uint _count;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources", Justification = ".NET doesn't support marshalling from/to a SafeHandle.  This field is required for the Win32 API.")]
        private IntPtr _pFields;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterNotifyOptionsType" /> struct.
        /// </summary>
        /// <param name="type">The <see cref="NotifyType" /> to subscribe to.</param>
        /// <param name="fields">The collection of <see cref="JobNotifyField" /> to subscribe to.</param>
        public PrinterNotifyOptionsType(NotifyType type, params JobNotifyField[] fields)
        {
            if (fields == null)
            {
                throw new ArgumentNullException(nameof(fields));
            }

            if (fields.Length < 1)
            {
                throw new ArgumentException("Fields must contain at least one element.", nameof(fields));
            }

            _reserved0 = 0;
            _reserved1 = 1;
            _reserved2 = 2;
            _count = (uint)fields.Length;
            _type = (ushort)type;

            _pFields = IntPtr.Zero;

            int size = Marshal.SizeOf<ushort>();
            _pFields = Marshal.AllocCoTaskMem(size * fields.Length);
            long ptrValue = _pFields.ToInt64();
            for (int i = 0; i < fields.Length; i++)
            {
                IntPtr ptr = new IntPtr(ptrValue);
                Marshal.StructureToPtr((ushort)fields[i], ptr, false);
                ptrValue += size;
            }
        }

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_pFields != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(_pFields);
                _pFields = IntPtr.Zero;
            }
        }
    }
}
