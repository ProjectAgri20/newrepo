using System;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Event args for the <see cref="WinSpoolPrintMonitor.JobNotificationReceived" /> event.
    /// </summary>
    internal class JobNotificationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the print job ID.
        /// </summary>
        public long JobId { get; }

        /// <summary>
        /// Gets the field that triggered this notification.
        /// </summary>
        public JobNotifyField Field { get; }

        /// <summary>
        /// Gets the value of the field.  The data type depends on the <see cref="Field" /> that was updated.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobNotificationEventArgs" /> class.
        /// </summary>
        /// <param name="data">The <see cref="PrinterNotifyInfoData" />.</param>
        internal JobNotificationEventArgs(PrinterNotifyInfoData data)
        {
            JobId = data.JobId;
            Field = data.Field;
            Value = data.Value;
        }
    }
}
