using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract (used for import/export) for EventLogCollector.
    /// </summary>
    [DataContract(Name = "ResourceUsageEntry", Namespace = "")]
    public class ResourceUsageEntry
    {
        /// <summary>
        /// The Resource Name.
        /// </summary>
        [DataMember]
        public string ResourceName { get; set; }

        /// <summary>
        /// The VirtualResourceMetadata Name.
        /// </summary>        
        [DataMember]
        public string MetadataName { get; set; }

        /// <summary>
        /// VirtualResourceMetadata Id.
        /// </summary>
        [DataMember]
        public Guid MetadataId { get; set; }
    }
}
