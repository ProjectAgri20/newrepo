using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Defines detailed information for the metadata associated with a defined resource
    /// </summary>
    [DataContract]
    [KnownType(typeof(LoadTesterMetadataDetail))]
    [KnownType(typeof(OfficeWorkerMetadataDetail))]
    [KnownType(typeof(LoadTesterExecutionPlan))]
    [KnownType(typeof(WorkerExecutionPlan))]
    public class ResourceMetadataDetail
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the metadata.
        /// </summary>
        [DataMember]
        public string MetadataType { get; set; }

        /// <summary>
        /// Gets or sets the Metadata version
        /// </summary>
        [DataMember]
        public string MetadataVersion { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [DataMember]
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets whether the metadata is enabled.
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the execution plan for this metadata component.
        /// </summary>
        /// <value>The plan.</value>
        [DataMember]
        public virtual IActivityExecutionPlan Plan { get; set; }
    }
}
