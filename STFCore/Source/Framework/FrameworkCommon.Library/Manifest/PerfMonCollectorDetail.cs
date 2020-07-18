using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Manifest details associated with the PerfMonCollector virtual resource
    /// </summary>
    [DataContract]
    public class PerfMonCollectorDetail : ResourceDetailBase
    {
        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        [DataMember]
        public string HostName { get; set; }

        /// <summary>
        /// Gets the unique names associated with the specific resource type.
        /// </summary>
        public override IEnumerable<string> UniqueNames
        {
            get { yield return Name; }
        }
    }
}