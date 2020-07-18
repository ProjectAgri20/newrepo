using System;
using System.IO;
using System.Runtime.Serialization;
using HP.ScalableTest.Xml;
using HP.ScalableTest.Data.EnterpriseTest.Model;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data contract (import/export) used to serialize an VirtualResourceMetadata object.
    /// </summary>
    [DataContract(Name = "ResourceMetadata", Namespace = "")]
    public class ResourceMetadataContract
    {
        /// <summary>
        /// The metadata.
        /// </summary>
        [DataMember]
        private string _data = string.Empty;

        /// <summary>
        /// The execution plan data.
        /// </summary>
        [DataMember]
        private string _executionPlan = string.Empty;

        /// <summary>
        /// Loads the ResourceMetadataContract from the specified VirtualResourceMetadata object.
        /// </summary>
        public void Load(VirtualResourceMetadata data)
        {
            Name = data.Name;
            MetadataId = data.VirtualResourceMetadataId;
            ResourceType = data.ResourceType;
            MetadataType = data.MetadataType;
            Metadata = data.Metadata;
            Enabled = data.Enabled;
            ExecutionPlan = data.ExecutionPlan;
        }

        /// <summary>
        /// The name of the VirtualResourceMetadata item.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The VirtualResourceMetadataId.
        /// </summary>
        [DataMember]
        public Guid MetadataId { get; set; }

        /// <summary>
        /// The VirtualResourceType.
        /// </summary>
        [DataMember]
        public string ResourceType { get; set; }

        /// <summary>
        /// The Metadata Type.
        /// </summary>
        [DataMember]
        public string MetadataType { get; set; }

        /// <summary>
        /// Whether the VirtualResourceMetada is enabled for execution.
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// The serialized metadata.
        /// </summary>
        public string Metadata
        {
            get { return _data.Decompress(); }
            set { _data = value.Compress(); }
        }

        /// <summary>
        /// The Execution plan data.
        /// </summary>
        public string ExecutionPlan
        {
            get { return _executionPlan.Decompress(); }
            set { _executionPlan = value.Compress(); }
        }

    }
}
