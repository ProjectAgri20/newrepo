using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used to build detail information on PerfMon Collectors for the <see cref="SystemManifest"/>
    /// </summary>
    internal class PerfMonCollectorDetailBuilder : DetailBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerfMonCollectorDetailBuilder"/> class.
        /// </summary>
        /// <param name="config">The builder.</param>
        /// <param name="resourcePacker">The resource packer.</param>
        public PerfMonCollectorDetailBuilder(SystemManifestAgent config, VirtualResourcePacker resourcePacker)
            : base(config, resourcePacker, VirtualResourceType.PerfMonCollector)
        {
        }

        /// <summary>
        /// Creates resource detail and inserts it into the manifest.
        /// </summary>
        /// <param name="resources">The resources.</param>
        /// <param name="manifest">The manifest.</param>
        internal override void AddToManifest(Collection<VirtualResource> resources, SystemManifest manifest)
        {
            // Iterate through each resource and create the manifest for the PerfMon Collector process.
            foreach (VirtualResource resource in resources)
            {
                PerfMonCollectorDetail detail = manifest.Resources.GetResource<PerfMonCollectorDetail>(resource.VirtualResourceId);
                if (detail == null)
                {
                    detail = CreateDetail(resource);
                    manifest.Resources.Add(detail);
                }
            }
        }

        public override ResourceDetailBase CreateBaseDetail(VirtualResource resource) => CreateDetail(resource);

        private PerfMonCollectorDetail CreateDetail(VirtualResource resource)
        {
            PerfMonCollector collector = resource as PerfMonCollector;

            PerfMonCollectorDetail detail = new PerfMonCollectorDetail
            {
                ResourceId = collector.VirtualResourceId,
                ResourceType = EnumUtil.Parse<VirtualResourceType>(collector.ResourceType),
                Name = collector.Name,
                Description = collector.Description,
                InstanceCount = collector.InstanceCount,
                Platform = collector.Platform,
                Enabled = collector.Enabled,
                HostName = collector.HostName
            };

            CreateMetadataDetail(resource, detail);
            return detail;
        }

        private void CreateMetadataDetail(VirtualResource resource, ResourceDetailBase detail)
        {
            foreach (var data in resource.VirtualResourceMetadataSet.Where(m => m.Enabled))
            {
                ResourceMetadataDetail metadata = new ResourceMetadataDetail()
                {
                    MetadataType = data.MetadataType,
                    Data = data.Metadata,
                    Id = data.VirtualResourceMetadataId,
                    Name = data.Name,
                    MetadataVersion = data.MetadataVersion
                };

                detail.MetadataDetails.Add(metadata);
            }
        }
    }
}