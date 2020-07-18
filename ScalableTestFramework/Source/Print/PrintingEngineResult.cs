using System;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// The result of a print operation using <see cref="PrintingEngine" />.
    /// </summary>
    public sealed class PrintingEngineResult
    {
        /// <summary>
        /// Gets the unique file name applied to the printed file.
        /// </summary>
        public string UniqueFileName { get; }

        /// <summary>
        /// Gets the unique ID applied to the printed file.
        /// </summary>
        public Guid UniqueFileId { get; }

        /// <summary>
        /// Gets the time when the <see cref="PrintingEngine" /> began processing the print request.
        /// </summary>
        public DateTimeOffset JobStartTime { get; }

        /// <summary>
        /// Gets the time when the <see cref="PrintingEngine" /> finished processing the print request.
        /// </summary>
        public DateTimeOffset JobEndTime { get; }

        /// <summary>
        /// Gets the time when the print was initiated.
        /// </summary>
        public DateTimeOffset PrintStartTime { get; }

        /// <summary>
        /// Gets the time when the application or operating system indicated printing was completed.
        /// </summary>
        public DateTimeOffset PrintEndTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintingEngineResult" /> class.
        /// </summary>
        /// <param name="uniqueFile">The <see cref="UniqueFile" /> that was printed.</param>
        /// <param name="filePrintResult">The <see cref="FilePrintResult" />.</param>
        /// <param name="jobStartTime">The time when the <see cref="PrintingEngine" /> began processing the print request.</param>
        /// <param name="jobEndTime">The time when the <see cref="PrintingEngine" /> finished processing the print request.</param>
        internal PrintingEngineResult(UniqueFile uniqueFile, FilePrintResult filePrintResult, DateTimeOffset jobStartTime, DateTimeOffset jobEndTime)
        {
            UniqueFileName = uniqueFile.Name;
            UniqueFileId = uniqueFile.Id;
            JobStartTime = jobStartTime;
            JobEndTime = jobEndTime;
            PrintStartTime = filePrintResult.PrintStartTime;
            PrintEndTime = filePrintResult.PrintEndTime;
        }
    }
}
