using System;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Office worker Metadata
    /// </summary>
    [KnownType(typeof(WorkerExecutionPlan))]
    [DataContract]
    public class OfficeWorkerMetadataDetail : ResourceMetadataDetail
    {
        /// <summary>
        /// Gets or sets the activity execution plan
        /// </summary>
        [DataMember]
        public override IActivityExecutionPlan Plan { get; set; }
    }
}
