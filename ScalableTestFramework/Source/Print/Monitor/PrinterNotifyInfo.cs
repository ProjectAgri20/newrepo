using System.Runtime.InteropServices;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Contains printer information returned by <see cref="WinSpoolPrintMonitor.FindNextPrinterChangeNotification" />.
    /// Data can be retrieved using <see cref="PrinterNotifyInfoReader" />.
    /// </summary>
    /// <remarks>
    /// Defined in WinSpool.h
    /// typedef struct _PRINTER_NOTIFY_INFO {
    ///     DWORD Version;
    ///     DWORD Flags;
    ///     DWORD Count;
    ///     PRINTER_NOTIFY_INFO_DATA aData[1];
    /// } PRINTER_NOTIFY_INFO, *PPRINTER_NOTIFY_INFO, *LPPRINTER_NOTIFY_INFO;
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PrinterNotifyInfo
    {
        private uint _version;
        private uint _status;
        private uint _count;

        /// <summary>
        /// Gets the state of the notification structure that produced this object.
        /// </summary>
        public PrinterNotifyInfoStatus Status => (PrinterNotifyInfoStatus)_status;

        /// <summary>
        /// Gets the number of <see cref="PrinterNotifyInfoData"/> objects contained within this one.
        /// </summary>
        public int Count => (int)_count;
    }
}
