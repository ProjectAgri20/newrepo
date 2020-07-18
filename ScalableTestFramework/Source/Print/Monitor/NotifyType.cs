namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Notification types that can be raised when monitoring the print subsystem.
    /// </summary>
    internal enum NotifyType
    {
        /// <summary>
        /// Notification for a change in the printer object.
        /// </summary>
        Printer = 0x00,

        /// <summary>
        /// Notification for a change in a print job.
        /// </summary>
        Job = 0x01
    }
}
