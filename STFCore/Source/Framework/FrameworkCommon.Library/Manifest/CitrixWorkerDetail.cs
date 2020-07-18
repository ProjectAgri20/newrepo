using System;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Manifest details class for Citrix Worker which is an Office Worker
    /// </summary>
    [DataContract]
    public class CitrixWorkerDetail : OfficeWorkerDetail
    {
        /// <summary>
        /// Gets or sets the citrix server hostname.
        /// </summary>
        [DataMember]
        public string ServerHostname { get; set; }

        /// <summary>
        /// Gets or sets the run mode which specifies if a worker or other published application will be started.
        /// </summary>
        [DataMember]
        public CitrixWorkerRunMode WorkerRunMode { get; set; }

        /// <summary>
        /// Gets or sets the citrix published application name.
        /// </summary>
        [DataMember]
        public string PublishedApp { get; set; }
    }
}
