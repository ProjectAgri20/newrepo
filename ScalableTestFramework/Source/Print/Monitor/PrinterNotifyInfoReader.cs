using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Helper class that reads <see cref="PrinterNotifyInfoData" /> from a <see cref="PrinterNotifyInfo" /> structure.
    /// </summary>
    internal sealed class PrinterNotifyInfoReader : IDisposable
    {
        private SafePrinterNotifyInfoHandle _infoHandle;

        /// <summary>
        /// Gets the <see cref="PrinterNotifyInfo" /> from this reader.
        /// </summary>
        public PrinterNotifyInfo Info { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="PrinterNotifyInfo" />
        /// has the <see cref="PrinterNotifyInfoStatus.InfoDiscarded" /> status.
        /// </summary>
        public bool InfoDiscarded => Info.Status.HasFlag(PrinterNotifyInfoStatus.InfoDiscarded);

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterNotifyInfoReader" /> class.
        /// </summary>
        public PrinterNotifyInfoReader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterNotifyInfoReader"/> class.
        /// </summary>
        /// <param name="infoHandle">The handle to a <see cref="PrinterNotifyInfo" />.</param>
        public PrinterNotifyInfoReader(SafePrinterNotifyInfoHandle infoHandle)
        {
            _infoHandle = infoHandle;
            Info = Marshal.PtrToStructure<PrinterNotifyInfo>(infoHandle.DangerousGetHandle());
        }

        /// <summary>
        /// Reads all <see cref="PrinterNotifyInfoData" /> from the <see cref="PrinterNotifyInfo" />.
        /// </summary>
        /// <returns>A collection of <see cref="PrinterNotifyInfoData" /> structures.</returns>
        public IEnumerable<PrinterNotifyInfoData> ReadInfoData()
        {
            for (int i = 0; i < Info.Count; i++)
            {
                yield return GetInfoData(i);
            }
        }

        private PrinterNotifyInfoData GetInfoData(int index)
        {
            // Need to figure out where the location must be set in order to find PrinterNotifyInfoData objects.
            // This location can be subtly different based on what bitness the OS is running.
            // For this reason, the true "size" of object is the marshal size, plus however many bytes gets us 
            // to the next memory boundary.  The size of IntPtr indicates what the memory boundary is for the given OS.
            int sizeOfInfo = Marshal.SizeOf<PrinterNotifyInfo>();
            sizeOfInfo += (sizeOfInfo % Marshal.SizeOf<IntPtr>());

            int sizeOfData = Marshal.SizeOf<PrinterNotifyInfoData>();
            sizeOfData += (sizeOfData % Marshal.SizeOf<IntPtr>());

            long location = _infoHandle.DangerousGetHandle().ToInt64() + sizeOfInfo + (sizeOfData * index);
            return Marshal.PtrToStructure<PrinterNotifyInfoData>(new IntPtr(location));
        }

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _infoHandle?.Dispose();
        }
    }
}
