using System;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// The result of a file printing operation.
    /// </summary>
    public sealed class FilePrintResult
    {
        /// <summary>
        /// Gets the time when the print was initiated.
        /// </summary>
        public DateTimeOffset PrintStartTime { get; }

        /// <summary>
        /// Gets the time when the application or operating system indicated printing was completed.
        /// </summary>
        public DateTimeOffset PrintEndTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePrintResult" /> class.
        /// </summary>
        /// <param name="printStartTime">The print start time.</param>
        /// <param name="printEndTime">The print end time.</param>
        public FilePrintResult(DateTimeOffset printStartTime, DateTimeOffset printEndTime)
        {
            PrintStartTime = printStartTime;
            PrintEndTime = printEndTime;
        }
    }
}
