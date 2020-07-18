using System;
using System.Globalization;

namespace HP.ScalableTest.Service.PrintMonitor
{
    /// <summary>
    /// Class that tracks a print queue's job count.
    /// </summary>
    internal class PrintQueueJobCount
    {
        /// <summary>
        /// Tracks when the queue job count was last obtained.
        /// </summary>
        private DateTime _obtained;

        /// <summary>
        /// Queue job count.
        /// </summary>
        private int _count;

        /// <summary>
        /// Get the queue job count.
        /// </summary>
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                _obtained = DateTime.Now;
                JobsPrinted = 0;
                RequestsGranted = 0;
            }
        }

        /// <summary>
        /// Tracks how many jobs have printed since the last time Count was updated.
        /// </summary>
        public int JobsPrinted { get; set; }

        /// <summary>
        /// How many print job requests have been granted since the last time Count was updated.
        /// </summary>
        public int RequestsGranted { get; set; }

        /// <summary>
        /// Is this queue job count expired.
        /// </summary>
        public bool Expired
        {
            get
            {
                return (DateTime.Now - _obtained) > new TimeSpan(0, 5, 0);
            }
        }

        public PrintQueueJobCount(int count)
        {
            Count = count;
        }

        /// <summary>
        /// Requests permission to print a page.
        /// If permission is given, then the <see cref="RequestsGranted"/> count will go up by one.
        /// </summary>
        /// <param name="maxJobCount">Max number of jobs allowed in the queue.</param>
        /// <returns>True if the request is granted.  False otherwise.</returns>
        public bool RequestPrint(int maxJobCount)
        {
            TraceFactory.Logger.Debug(string.Format(CultureInfo.InvariantCulture, "Count: {0}; RequestsGranted: {1}; JobsPrinted: {2}", Count, RequestsGranted, JobsPrinted));

            if (Count + RequestsGranted - JobsPrinted > maxJobCount)
            {
                return false;
            }

            // This means the job can print.
            TraceFactory.Logger.Debug("Go ahead");
            RequestsGranted++;
            return true;
        }
    }
}
