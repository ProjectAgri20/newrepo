using System.Runtime.Serialization;

namespace HP.ScalableTest.PluginSupport.Print
{
    /// <summary>
    /// Contains data needed to execute a printing activity through the Driver Configuration Print plugin.
    /// </summary>
    [DataContract]
    public class PrintingActivityData
    {
        /// <summary>
        /// Gets or sets whether the list of documents to print is shuffled prior to each run.
        /// </summary>
        [DataMember]
        public bool ShuffleDocuments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to print a separator between jobs.
        /// </summary>
        /// <value>The print job separator.</value>
        [DataMember]
        public bool PrintJobSeparator { get; set; }

        /// <summary>
        /// Gets or sets whether Job Throttling is turned on.  If this option is turned on,
        /// then the print activity will only send print jobs as long as the queue in question has less jobs than what
        /// is specified in <see cref="MaxJobsInQueue"/>
        /// </summary>
        [DataMember]
        public bool JobThrottling { get; set; }

        /// <summary>
        /// Gets or sets the total number of jobs allowed in the print queue.  This parameter is used only if
        /// <see cref="JobThrottling"/> is set to true.
        /// </summary>
        [DataMember]
        public int MaxJobsInQueue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintingActivityData"/> class.
        /// </summary>
        public PrintingActivityData()
        {
            MaxJobsInQueue = 1;
        }
    }
}
