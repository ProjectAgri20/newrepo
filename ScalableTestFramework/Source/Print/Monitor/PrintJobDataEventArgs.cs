using System;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Event args containing a <see cref="PrintJobData" /> object.
    /// </summary>
    public sealed class PrintJobDataEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the print job data.
        /// </summary>
        public PrintJobData Job { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintJobDataEventArgs" /> class.
        /// </summary>
        /// <param name="job">The job.</param>
        public PrintJobDataEventArgs(PrintJobData job)
        {
            Job = job;
        }
    }
}
