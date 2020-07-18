using System;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// The state of the notification structure that produced a <see cref="PrinterNotifyInfo" /> object.
    /// </summary>
    [Flags]
    internal enum PrinterNotifyInfoStatus
    {
        /// <summary>
        /// No status specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Some notifications had to be discarded.
        /// </summary>
        InfoDiscarded = 1
    }
}
