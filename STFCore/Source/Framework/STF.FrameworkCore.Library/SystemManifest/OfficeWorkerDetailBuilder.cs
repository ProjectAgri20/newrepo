using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used to build detail information on Office Workers for the <see cref="SystemManifest"/>
    /// </summary>
    internal class OfficeWorkerDetailBuilder : DetailBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorkerDetailBuilder"/> class.
        /// </summary>
        /// <param name="config">The builder.</param>
        /// <param name="resourcePacker">The resource packer.</param>
        public OfficeWorkerDetailBuilder(SystemManifestAgent config, VirtualResourcePacker resourcePacker)
            : base(config, resourcePacker, VirtualResourceType.OfficeWorker)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorkerDetailBuilder"/> class.
        /// </summary>
        /// <param name="config">The builder.</param>
        /// <param name="resourcePacker">The resource packer.</param>
        /// <param name="resourceType">Type of the resource.</param>
        public OfficeWorkerDetailBuilder(SystemManifestAgent config, VirtualResourcePacker resourcePacker, VirtualResourceType resourceType)
            : base(config, resourcePacker, resourceType)
        {
        }

        /// <summary>
        /// Creates resource detail and inserts it into the manifest.
        /// </summary>
        /// <param name="resources">The resources.</param>
        /// <param name="manifest">The manifest.</param>
        internal override void AddToManifest(Collection<VirtualResource> resources, SystemManifest manifest)
        {
            LoadResources<OfficeWorkerDetail>(resources, manifest);
        }

        internal virtual OfficeWorkerCredential AddCredential(VirtualResource resource, OfficeWorkerDetail detail)
        {
            // Get the next credential and add it to the OW definition
            OfficeWorkerCredential credential = ManifestAgent.UserAccounts.NextUserCredential(((OfficeWorker)resource).UserPool);
            credential.Port = detail.CommandPortOffset + credential.Port;
            credential.ResourceInstanceId = credential.UserName;       
            return credential;
        }

        internal virtual void AddExternalCredentials(OfficeWorkerCredential domainCredential, OfficeWorkerDetail officeWorkerDetail)
        {
            using (AssetInventoryContext context = new AssetInventoryContext(DbConnect.AssetInventoryConnectionString))
            {
                foreach (ExternalCredential extCredential in context.ExternalCredentials.Where(x => x.DomainUserName == domainCredential.UserName))
                {
                    officeWorkerDetail.ExternalCredentials.Add(new ExternalCredentialDetail(extCredential.UserName, extCredential.Password, extCredential.ExternalCredentialType, extCredential.DomainUserName));
                }
            }
        }

        protected void LoadResources<T>(Collection<VirtualResource> resources, SystemManifest manifest) where T : OfficeWorkerDetail
        {
            // Iterate through each resource and create the office worker process
            foreach (var resource in resources)
            {
                // First determine if the resource already exists in the Resource collection.  If not, then
                // have VirtualResource create it, then update it with other data and add it to the list.
                var detail = manifest.Resources.GetResource<T>(resource.VirtualResourceId);
                if (detail == null)
                {
                    detail = (T)CreateDetail(resource);
                    detail.OfficeWorkerCount = ResourcePacker.TotalResourceCount(ResourceType);
                    detail.StartIndex = ManifestAgent.UserAccounts.StartIndex(((OfficeWorker)resource).UserPool);
                    detail.UserNameFormat = ManifestAgent.UserAccounts.UserFormat(((OfficeWorker)resource).UserPool);
                    detail.CommandPortOffset = 40000;
                    manifest.Resources.Add(detail);
                }

                // Get the next credential and add it to the OW definition
                OfficeWorkerCredential credential = AddCredential(resource, detail);
                detail.UserCredentials.Add(credential);

                // Add any external credentials associated with the OW credential
                AddExternalCredentials(credential, detail);
            }
        }

        /// <summary>
        /// Creates a <see cref="ResourceDetailBase"/> from a virtural resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        public override ResourceDetailBase CreateBaseDetail(VirtualResource resource) => CreateDetail(resource);

        /// <summary>
        /// Creates an <see cref="OfficeWorkerDetail"/> from the specified resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        public virtual OfficeWorkerDetail CreateDetail(VirtualResource resource)
        {
            OfficeWorkerDetail detail = new OfficeWorkerDetail();
            CreateBaseWorkerDetail(resource as OfficeWorker, detail);
            CreateMetadataDetail(resource, detail);
            return detail;
        }

        protected void CreateBaseWorkerDetail(OfficeWorker resource, OfficeWorkerDetail detail)
        {
            detail.ResourceId = resource.VirtualResourceId;
            detail.ResourceType = EnumUtil.Parse<VirtualResourceType>(resource.ResourceType);
            detail.Name = resource.Name;
            detail.Description = resource.Description;
            detail.InstanceCount = resource.InstanceCount;
            detail.Platform = resource.Platform;
            detail.Enabled = resource.Enabled;

            detail.RandomizeActivities = resource.RandomizeActivities;
            detail.RandomizeStartupDelay = resource.RandomizeStartupDelay;
            detail.MinStartupDelay = resource.MinStartupDelay;
            detail.MaxStartupDelay = resource.MaxStartupDelay;
            detail.RandomizeActivityDelay = resource.RandomizeActivityDelay;
            detail.MinActivityDelay = resource.MinActivityDelay;
            detail.MaxActivityDelay = resource.MaxActivityDelay;
            detail.RepeatCount = resource.RepeatCount;
            detail.ExecutionMode = resource.ExecutionMode;
            detail.DurationTime = resource.DurationTime;
            detail.SecurityGroups = resource.SecurityGroups;
            detail.ResourcesPerVM = resource.ResourcesPerVM ?? 1;

            if (resource.ExecutionMode == ExecutionMode.Scheduled)
            {
                detail.ExecutionSchedule = resource.ExecutionSchedule;
            }
        }

        protected void CreateMetadataDetail(VirtualResource resource, ResourceDetailBase detail)
        {
            Dictionary<int, OfficeWorkerMetadataDetail> orderedDetails = new Dictionary<int, OfficeWorkerMetadataDetail>();

            foreach (var data in resource.VirtualResourceMetadataSet)
            {
                OfficeWorkerMetadataDetail metadata = new OfficeWorkerMetadataDetail()
                {
                    MetadataType = data.MetadataType,
                    Data = data.Metadata,

                    Plan = data.ExecutionPlan != null
                        ? LegacySerializer.DeserializeDataContract<WorkerExecutionPlan>(data.ExecutionPlan)
                        : new WorkerExecutionPlan(),

                    Id = data.VirtualResourceMetadataId,
                    Name = data.Name,
                    MetadataVersion = data.MetadataVersion,
                    Enabled = data.Enabled,
                };

                // Offset the key by 100, this is a bunch but it will guarantee (or should) that
                // if for some reason the same order number exists in two plans that they 
                // won't conflict.  While the ordered list contains the key, it will keep
                // adding one until it's found an open spot.  Using 100 means we could have up
                // to 100 entries with the same order number and still resolve them.  Of course this
                // doesn't guarantee any ultimate order to the metadata, but in most cases when
                // the order value is unique, this will just ensure the items are added in numerical
                // order so that the serialized XML shows them in order as well.
                int key = metadata.Plan.Order * 100;

                while (orderedDetails.ContainsKey(key))
                {
                    key++;
                }
                orderedDetails.Add(key, metadata);
            }

            // Add the metadata to the manifest in order so that the XML shows them in order.
            foreach (int key in orderedDetails.Keys.OrderBy(x => x))
            {
                detail.MetadataDetails.Add(orderedDetails[key]);
            }
        }

    }
}