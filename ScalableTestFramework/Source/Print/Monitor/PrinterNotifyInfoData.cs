using System;
using System.Runtime.InteropServices;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Identifies a job or printer information field and provides the current data for that field.
    /// </summary>
    /// <remarks>
    /// Defined in WinSpool.h
    /// typedef struct _PRINTER_NOTIFY_INFO_DATA {
    ///     WORD Type;
    ///     WORD Field;
    ///     DWORD Reserved;
    ///     DWORD Id;
    ///     union {
    ///         DWORD adwData[2];
    ///         struct {
    ///             DWORD  cbBuf;
    ///             LPVOID pBuf;
    ///         } Data;
    ///     } NotifyData;
    /// } PRINTER_NOTIFY_INFO_DATA, *PPRINTER_NOTIFY_INFO_DATA, *LPPRINTER_NOTIFY_INFO_DATA;
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PrinterNotifyInfoData
    {
        private ushort _type;
        private ushort _field;
        private uint _reserved;
        private uint _id;
        private PrinterNotifyInfoNotifyData NotifyData;

        /// <summary>
        /// Gets the <see cref="NotifyType" /> contained in this object.
        /// </summary>
        public NotifyType NotifyType => (NotifyType)_type;

        /// <summary>
        /// Gets the <see cref="JobNotifyField" /> that was updated.
        /// Only valid if <see cref="NotifyType" /> is <see cref="NotifyType.Job" />.
        /// </summary>
        public JobNotifyField Field => (JobNotifyField)_field;

        /// <summary>
        /// The print job identifier.
        /// Only valid of <see cref="NotifyType" /> is <see cref="NotifyType.Job" />.
        /// </summary>
        public long JobId => _id;

        /// <summary>
        /// Gets an <see cref="object" /> representing the data value.
        /// The type of object returned varies depending on the <see cref="JobNotifyField" /> this object represents.
        /// </summary>
        public object Value
        {
            get
            {
                switch (Field)
                {
                    case JobNotifyField.PrinterName:
                    case JobNotifyField.MachineName:
                    case JobNotifyField.PortName:
                    case JobNotifyField.UserName:
                    case JobNotifyField.NotifyName:
                    case JobNotifyField.DataType:
                    case JobNotifyField.PrintProcessor:
                    case JobNotifyField.Parameters:
                    case JobNotifyField.DriverName:
                    case JobNotifyField.StatusString:
                    case JobNotifyField.Document:
                        // This is a string-type field.
                        return NotifyData.Data.BufferAsString;

                    case JobNotifyField.Status:
                        // This is a bit field.
                        return Enum.ToObject(typeof(JobStatus), NotifyData.DataA);

                    case JobNotifyField.Submitted:
                        // DateTime field.
                        SystemTime systemtime = NotifyData.Data.BufferAsSystemTime;
                        DateTime utcDateTime = new DateTime(systemtime.Year, systemtime.Month, systemtime.Day, systemtime.Hour, systemtime.Minute, systemtime.Second, systemtime.Milliseconds, DateTimeKind.Utc);
                        return new DateTimeOffset(utcDateTime).ToLocalTime();

                    case JobNotifyField.Priority:
                    case JobNotifyField.Position:
                    case JobNotifyField.StartTime:
                    case JobNotifyField.UntilTime:
                    case JobNotifyField.Time:
                    case JobNotifyField.TotalPages:
                    case JobNotifyField.PagesPrinted:
                    case JobNotifyField.TotalBytes:
                    case JobNotifyField.BytesPrinted:
                        return (long)NotifyData.DataA;

                    default:
                        throw new InvalidOperationException("Unsupported field: " + Field);
                }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct PrinterNotifyInfoNotifyData
        {
            [FieldOffset(0)]
            public uint DataA;

            [FieldOffset(4)]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Must match layout in external DLL.")]
            public uint DataB;

            [FieldOffset(0)]
            public PrinterNotifyInfoNotifyDataData Data;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PrinterNotifyInfoNotifyDataData
        {
            public uint Count;
            public IntPtr Buffer;

            public string BufferAsString => Marshal.PtrToStringUni(Buffer);
            public SystemTime BufferAsSystemTime => Marshal.PtrToStructure<SystemTime>(Buffer);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SystemTime
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Milliseconds;
        }
    }
}
