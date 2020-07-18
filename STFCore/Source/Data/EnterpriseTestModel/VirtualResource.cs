using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    [ObjectFactory(VirtualResourceType.MachineReservation)]
    public partial class VirtualResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResource"/> class.
        /// </summary>
        public VirtualResource()
        {
            VirtualResourceId = SequentialGuid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResource"/> class.
        /// </summary>
        public VirtualResource(string resourceType)
            : this()
        {
            Name = "{0} {1}".FormatWith(resourceType, DateTime.Now.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture));
            ResourceType = resourceType;
            InstanceCount = 1;
            Platform = string.Empty;
            Enabled = true;
            ResourcesPerVM = 15;
        }

        /// <summary>
        /// Selects a <see cref="VirtualResource"/> by its id.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static VirtualResource Select(EnterpriseTestEntities entities, Guid id)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from n in entities.VirtualResources
                    where n.VirtualResourceId == id
                    select n).FirstOrDefault();
        }

        /// <summary>
        /// Selects all <see cref="VirtualResource"/> objects with ids in the specified list.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public static IQueryable<VirtualResource> Select(EnterpriseTestEntities entities, IEnumerable<Guid> ids)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return from n in entities.VirtualResources
                   where ids.Contains(n.VirtualResourceId)
                   select n;
        }

        /// <summary>
        /// Gets a string representation of the duration.
        /// </summary>
        public virtual string DurationString
        {
            get { return !Runtime.Equals(TimeSpan.Zero) ? Runtime.ToString() : "N/A"; }
        }        

        #region Load From Detail

        /// <summary>
        /// Loads the specified VirtualResourceDetail into the specified EnterpriseScenario.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scenario">The <see cref="EnterpriseScenario"/></param>
        /// <param name="detail">The metadata detail</param>
        public void LoadDetail<T>(EnterpriseScenario scenario, T detail) where T : ResourceDetailBase
        {
            VirtualResourceId = SequentialGuid.NewGuid();
            EnterpriseScenario = scenario;
            EnterpriseScenarioId = scenario.EnterpriseScenarioId;
            ResourceType = detail.ResourceType.ToString();
            Name = detail.Name;
            Description = detail.Description;
            InstanceCount = detail.InstanceCount;
            Platform = detail.Platform;
            Enabled = detail.Enabled;

            foreach (var metadataDetail in detail.MetadataDetails)
            {
                VirtualResourceMetadata metadata = new VirtualResourceMetadata(ResourceType, metadataDetail.MetadataType)
                {
                    VirtualResource = this,
                    VirtualResourceId = VirtualResourceId,
                    ExecutionPlan = GetExecutionPlan(metadataDetail),
                    VirtualResourceMetadataId = metadataDetail.Id,
                    Name = metadataDetail.Name,
                    Metadata = metadataDetail.Data,
                    MetadataVersion = metadataDetail.MetadataVersion,
                    Enabled = metadataDetail.Enabled
                };

                VirtualResourceMetadataSet.Add(metadata);
            }

            LoadChildDetail(detail);            
        }

        protected virtual void LoadChildDetail(ResourceDetailBase detail)
        {
            // Intentionally left blank.
        }

        protected virtual string GetExecutionPlan(ResourceMetadataDetail detail)
        {
            return LegacySerializer.SerializeDataContract((WorkerExecutionPlan)detail.Plan).ToString();

        }

        #endregion

        /// <summary>
        /// Creates a copy of this <see cref="VirtualResource"/>.
        /// </summary>
        /// <returns></returns>
        public VirtualResource Copy()
        {
            // Create a new resource of the same type as this one using reflection
            VirtualResource newResource = (VirtualResource)Activator.CreateInstance(this.GetType());
            newResource.CopyResourceProperties(this);

            // Copy over all metadata items
            foreach (VirtualResourceMetadata metadata in this.VirtualResourceMetadataSet)
            {
                VirtualResourceMetadata copiedMetadata = metadata.Copy(true);
                copiedMetadata.VirtualResourceId = newResource.VirtualResourceId;
                newResource.VirtualResourceMetadataSet.Add(copiedMetadata);
            }

            return newResource;
        }


        /// <summary>
        /// Copies a Virtual Resource and all of its 'configuration data' - but does NOT
        /// copy the Metadata items.
        /// </summary>
        /// <returns>The Copy of the Virtual Resource</returns>
        public VirtualResource ShallowCopy()
        {
            // Create a new resource of the same type as this one using reflection
            VirtualResource newResource = (VirtualResource)Activator.CreateInstance(this.GetType());
            newResource.CopyResourceProperties(this);
            return newResource;
        }


        /// <summary>
        /// Copies all resource-specific properties from the target VirtualResource to this instance.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <remarks>
        /// This method is used when committing changes to an existing resource or creating
        /// a copy of a resource.  It should copy any properties that are specific to the
        /// child resource type from the target resource to this instance.
        /// Properties defined by the VirtualResource base class do not need to be copied.
        /// </remarks>
        public virtual void CopyResourceProperties(VirtualResource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }

            Name = resource.Name;
            Description = resource.Description;
            ResourceType = resource.ResourceType;
            InstanceCount = resource.InstanceCount;
            Platform = resource.Platform;
            Enabled = resource.Enabled;
        }

        /// <summary>
        /// The expected run time of this instance.  Returns 0 if no run time is specified.
        /// </summary>
        public virtual TimeSpan Runtime
        {
            get { return TimeSpan.Zero; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Checks the integrity of the data for this virtual resource.
        /// </summary>
        /// <returns>A list of strings, each of which provides information about a data integrity issue.</returns>
        public virtual IEnumerable<string> ValidateData()
        {
            // By default, there are no issues
            yield break;
        }

        /// <summary>
        /// Gets the expanded definitions for this resource.
        /// </summary>
        /// <value>
        /// A collection of virtual resource definitions that have been expanded if 
        /// it applies to the specific resource type
        /// </value>
        /// <remarks>
        /// 
        /// </remarks>
        public virtual IEnumerable<VirtualResource> ExpandedDefinitions
        {
            get
            {
                for (int i = 0; i < InstanceCount; i++)
                {
                    yield return this;
                }
            }
        }
    }
}