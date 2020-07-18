namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Job information fields that can be monitored by a printer change notification object.
    /// </summary>
    /// <remarks>
    /// Descriptions pulled from MSDN documentation for PRINTER_NOTIFY_INFO_DATA structure.
    /// </remarks>
    internal enum JobNotifyField
    {
        /// <summary>
        /// The name of the printer for which the job is spooled.
        /// </summary>
        PrinterName = 0x00,

        /// <summary>
        /// The name of the machine that created the print job.
        /// </summary>
        MachineName = 0x01,

        /// <summary>
        /// The port used to transmit data to the printer.
        /// </summary>
        PortName = 0x02,

        /// <summary>
        /// The name of the user who sent the print job.
        /// </summary>
        UserName = 0x03,

        /// <summary>
        /// The name of the user who should be notified when the job has been printed.
        /// </summary>
        NotifyName = 0x04,

        /// <summary>
        /// The type of data used to record the print job.
        /// </summary>
        DataType = 0x05,

        /// <summary>
        /// The name of the print processor used to record the print job.
        /// </summary>
        PrintProcessor = 0x06,

        /// <summary>
        /// Print-processor parameters.
        /// </summary>
        Parameters = 0x07,

        /// <summary>
        /// The name of the printer driver that should be used to process the print job.
        /// </summary>
        DriverName = 0x08,

        /// <summary>
        /// Device-initialization and environment data for the printer driver.
        /// </summary>
        DevMode = 0x09,

        /// <summary>
        /// The job status, which should be of type <see cref="JobStatus"/>.
        /// </summary>
        Status = 0x0A,

        /// <summary>
        /// String that specifies the status of the print job.
        /// </summary>
        StatusString = 0x0B,

        /// <summary>
        /// Not supported.
        /// </summary>
        SecurityDescriptor = 0x0C,

        /// <summary>
        /// The name of the print job (for example, "MS-WORD: Review.doc").
        /// </summary>
        Document = 0x0D,

        /// <summary>
        /// The job priority.
        /// </summary>
        Priority = 0x0E,

        /// <summary>
        /// The job's position in the print queue.
        /// </summary>
        Position = 0x0F,

        /// <summary>
        /// The time when the job was submitted.
        /// </summary>
        Submitted = 0x10,

        /// <summary>
        /// The earliest time that the job can be printed, specified in minutes elapsed past 12:00 A.M.
        /// </summary>
        StartTime = 0x11,

        /// <summary>
        /// The latest time that the job can be printed, specified in minutes elapsed past 12:00 A.M.
        /// </summary>
        UntilTime = 0x12,

        /// <summary>
        /// The total time, in seconds, that has elapsed since the job began printing.
        /// </summary>
        Time = 0x13,

        /// <summary>
        /// The size, in pages, of the job.
        /// </summary>
        TotalPages = 0x14,

        /// <summary>
        /// The number of pages that have printed.
        /// </summary>
        PagesPrinted = 0x15,

        /// <summary>
        /// The size, in bytes, of the job.
        /// </summary>
        TotalBytes = 0x16,

        /// <summary>
        /// The number of bytes that have been printed on this job.
        /// </summary>
        BytesPrinted = 0x17,
    }
}
