using System;
using System.Printing;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Tracks information about a print queue that is being monitored.
    /// Used to cache queue information to reduce the number of calls required to the print spooler.
    /// </summary>
    public sealed class MonitoredQueueInfoCache
    {
        /// <summary>
        /// Gets the name of the monitored print queue.
        /// </summary>
        public string QueueName { get; }

        /// <summary>
        /// Gets or sets the name of the hosting print server.
        /// </summary>
        public string PrintServer { get; set; }

        /// <summary>
        /// Gets or sets the operating system of the hosting print server.
        /// </summary>
        public string PrintServerOS { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the queue is configured to render jobs on the client.
        /// </summary>
        public bool? RenderOnClient { get; set; }

        /// <summary>
        /// Gets or sets the default color mode.
        /// </summary>
        public string ColorMode { get; set; }

        /// <summary>
        /// Gets or sets the default number of copies.
        /// </summary>
        public int? Copies { get; set; }

        /// <summary>
        /// Gets or sets the default number of pages up.
        /// </summary>
        public int? NumberUp { get; set; }

        /// <summary>
        /// Gets or sets the default duplex setting.
        /// </summary>
        public string Duplex { get; set; }

        /// <summary>
        /// Gets or sets the date when the queue settings were last retrieved.
        /// </summary>
        public DateTime QueueSettingsRetrieved { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitoredQueueInfoCache" /> class.
        /// </summary>
        /// <param name="queueName">The print queue name.</param>
        public MonitoredQueueInfoCache(string queueName)
        {
            QueueName = queueName;
            QueueSettingsRetrieved = DateTime.MinValue;
        }

        /// <summary>
        /// Updates the cached queue settings from the specified <see cref="PrintQueue" /> object.
        /// </summary>
        /// <param name="queue">The <see cref="PrintQueue" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="queue" /> is null.</exception>
        public void Refresh(PrintQueue queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            ColorMode = (queue.DefaultPrintTicket.OutputColor ?? OutputColor.Unknown).ToString();
            Copies = queue.DefaultPrintTicket.CopyCount ?? 0;
            NumberUp = queue.DefaultPrintTicket.PagesPerSheet ?? 0;
            Duplex = (queue.DefaultPrintTicket.Duplexing ?? Duplexing.Unknown).ToString();
        }
    }
}
