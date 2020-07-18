using System;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Changes that can occur within the print subsystem.
    /// </summary>
    /// <remarks>
    /// Descriptions pulled from MSDN documentation for FindNextPrinterChangeNotification function.
    /// </remarks>
    [Flags]
    internal enum PrinterChanges : long
    {
        /// <summary>
        /// No change specified.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// A printer was added to the server.
        /// </summary>
        AddPrinter = 0x00000001,

        /// <summary>
        /// A printer was set.
        /// </summary>
        SetPrinter = 0x00000002,

        /// <summary>
        /// A printer was deleted.
        /// </summary>
        DeletePrinter = 0x00000004,

        /// <summary>
        /// A printer connection has failed.
        /// </summary>
        FailedConnectionPrinter = 0x00000008,

        /// <summary>
        /// Not supported.
        /// </summary>
        UnsupportedBit04 = 0x00000010,

        /// <summary>
        /// Not supported.
        /// </summary>
        UnsupportedBit05 = 0x00000020,

        /// <summary>
        /// Not supported.
        /// </summary>
        UnsupportedBit06 = 0x00000040,

        /// <summary>
        /// Not supported.
        /// </summary>
        UnsupportedBit07 = 0x00000080,

        /// <summary>
        /// Combination of <see cref="AddPrinter" />, <see cref="SetPrinter" />, and <see cref="DeletePrinter" />.
        /// </summary>
        Printer = 0x000000FF,

        /// <summary>
        /// A print job was sent to the printer.
        /// </summary>
        AddJob = 0x00000100,

        /// <summary>
        /// A job was set.
        /// </summary>
        SetJob = 0x00000200,

        /// <summary>
        /// A job was deleted.
        /// </summary>
        DeleteJob = 0x00000400,

        /// <summary>
        /// Job data was written.
        /// </summary>
        WriteJob = 0x00000800,

        /// <summary>
        /// Not supported.
        /// </summary>
        UnsupportedBit12 = 0x00001000,

        /// <summary>
        /// Not supported.
        /// </summary>
        UnsupportedBit13 = 0x00002000,

        /// <summary>
        /// Not supported.
        /// </summary>
        UnsupportedBit14 = 0x00004000,

        /// <summary>
        /// Not supported.
        /// </summary>
        UnsupportedBit15 = 0x00008000,

        /// <summary>
        /// Combination of <see cref="AddJob" />, <see cref="SetJob" />, <see cref="DeleteJob" />, and <see cref="WriteJob" />.
        /// </summary>
        Job = 0x0000FF00,

        /// <summary>
        /// A form was added to the server.
        /// </summary>
        AddForm = 0x00010000,

        /// <summary>
        /// A form was set on the server.
        /// </summary>
        SetForm = 0x00020000,

        /// <summary>
        /// A form was deleted from the server.
        /// </summary>
        DeleteForm = 0x00040000,

        /// <summary>
        /// Not supported.
        /// </summary>
        UnsupportedBit19 = 0x00080000,

        /// <summary>
        /// Combination of <see cref="AddForm" />, <see cref="SetForm" />, and <see cref="DeleteForm" />.
        /// </summary>
        Form = 0x00070000,

        /// <summary>
        /// A port or monitor was added to the server.
        /// </summary>
        AddPort = 0x00100000,

        /// <summary>
        /// A port was configured on the server.
        /// </summary>
        ConfigurePort = 0x00200000,

        /// <summary>
        /// A port or monitor was deleted from the server.
        /// </summary>
        DeletePort = 0x00400000,

        /// <summary>
        /// Unsupported.
        /// </summary>
        UnsupportedBit23 = 0x00800000,

        /// <summary>
        /// Combination of <see cref="AddPort" />, <see cref="ConfigurePort" />, and <see cref="DeletePort" />.
        /// </summary>
        Port = 0x00700000,

        /// <summary>
        /// A print processor was added to the server.
        /// </summary>
        AddPrintProcessor = 0x01000000,

        /// <summary>
        /// Not supported.
        /// </summary>
        UnsupportedBit25 = 0x02000000,

        /// <summary>
        /// A print processor was deleted from the server.
        /// </summary>
        DeletePrintProcessor = 0x04000000,

        /// <summary>
        /// Not supported.
        /// </summary>
        UnsupportedBit27 = 0x08000000,

        /// <summary>
        /// Combination of <see cref="AddPrintProcessor" /> and <see cref="DeletePrintProcessor" />.
        /// </summary>
        PrintProcessor = 0x07000000,

        /// <summary>
        /// A printer driver was added to the server.
        /// </summary>
        AddPrinterDriver = 0x10000000,

        /// <summary>
        /// A printer driver was set.
        /// </summary>
        SetPrinterDriver = 0x20000000,

        /// <summary>
        /// A printer driver was deleted from the server.
        /// </summary>
        DeletePrinterDriver = 0x40000000,

        /// <summary>
        /// Combination of <see cref="AddPrinterDriver" />, <see cref="SetPrinterDriver" />, and <see cref="DeletePrinterDriver" />.
        /// </summary>
        PrinterDriver = 0x70000000,

        /// <summary>
        /// The job timed out.
        /// </summary>
        Timeout = 0x80000000,

        /// <summary>
        /// All bits set.
        /// </summary>
        All = 0x7777FFFF
    }
}
