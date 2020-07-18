using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.SyncPoint
{
    /// <summary>
    /// Activity data for the SyncPoint plugin.
    /// </summary>
    [DataContract]
    internal class SyncPointActivityData
    {
        /// <summary>
        /// Gets or sets the synchronization event name.
        /// </summary>
        [DataMember]
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SyncPointAction" /> to perform.
        /// </summary>
        [DataMember]
        public SyncPointAction Action { get; set; }
    }
}
