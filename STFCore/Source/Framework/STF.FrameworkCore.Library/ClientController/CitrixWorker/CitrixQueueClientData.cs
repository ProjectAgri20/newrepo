using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Contains Citrix Queue Client Data
    /// </summary>
    [DataContract]
    public class CitrixQueueClientData
    {
        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>The name of the host.</value>
        [DataMember]
        public string HostName { get; set; }
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// Gets or sets the session start.
        /// </summary>
        /// <value>The session start.</value>
        [DataMember]
        public DateTime SessionStart { get; set; }
        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>The name of the queue.</value>
        [DataMember]
        public string QueueName { get; set; }
        /// <summary>
        /// Gets or sets the print driver.
        /// </summary>
        /// <value>The print driver.</value>
        [DataMember]
        public string PrintDriver { get; set; }
        /// <summary>
        /// Gets or sets the session id.
        /// </summary>
        /// <value>The session id.</value>
        [DataMember]
        public string SessionId { get; set; }
        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        /// <value>The end point.</value>
        [DataMember]
        public Uri EndPoint { get; set; }
        /// <summary>
        /// Gets or sets the startup delay.
        /// </summary>
        /// <value>The startup delay.</value>
        [DataMember]
        public TimeSpan StartupDelay { get; set; }

    }
}
