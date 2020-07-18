using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest.Model;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data contract (import/export) used to serialize an VirtualResource object.
    /// </summary>
    [DataContract(Name = "ResourceContract", Namespace = "")]
    [KnownType(typeof(OfficeWorkerContract))]
    [KnownType(typeof(PerfMonCollectorContract))]
    [KnownType(typeof(EventLogCollectorContract))]
    [KnownType(typeof(MachineReservationContract))]
    [KnownType(typeof(LoadTesterContract))]
    public class ResourceContract
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ResourceContract()
        {
            MetadataContract = new Collection<ResourceMetadataContract>();
        }

        /// <summary>
        /// The Name of the Virtual Resource.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The Virtual Resource Id.
        /// </summary>
        [DataMember]
        public Guid VirtualResourceId { get; set; }

        /// <summary>
        /// The Desctiption.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// The Resource Type.
        /// </summary>
        [DataMember]
        public string ResourceType { get; set; }

        /// <summary>
        /// The Instance Count.
        /// </summary>
        [DataMember]
        public int InstanceCount { get; set; }

        /// <summary>
        /// The VM Platform.
        /// </summary>
        [DataMember]
        public string Platform { get; set; }

        /// <summary>
        /// Number of Resources per VM.
        /// </summary>
        [DataMember]
        public int? ResourcesPerVM { get; set; }

        /// <summary>
        /// Whether the VirtualResource is enabled for execution.
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// The ResourceMetadataContracts associated with this ResourceContract.
        /// </summary>
        [DataMember]
        public Collection<ResourceMetadataContract> MetadataContract { get; private set; }

        /// <summary>
        /// Loads the ResourceContract from the specified VirtualResource object.
        /// </summary>
        /// <param name="resource"></param>
        public virtual void Load(VirtualResource resource)
        {
            VirtualResourceId = resource.VirtualResourceId;
            Name = resource.Name;
            Description = resource.Description;
            ResourceType = resource.ResourceType;
            InstanceCount = resource.InstanceCount;
            Platform = resource.Platform;
            ResourcesPerVM = resource.ResourcesPerVM;
            Enabled = resource.Enabled;
        }
    }

}
