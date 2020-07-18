using System.Collections.ObjectModel;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used to build detail information on Event Log Collectors for the <see cref="SystemManifest"/>
    /// </summary>
    internal class EventLogCollectorDetailBuilder : DetailBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogCollectorDetailBuilder"/> class.
        /// </summary>
        /// <param name="config">The builder.</param>
        /// <param name="resourcePacker">The resource packer.</param>
        public EventLogCollectorDetailBuilder(SystemManifestAgent config, VirtualResourcePacker resourcePacker)
            : base(config, resourcePacker, VirtualResourceType.EventLogCollector)
        {
        }

        /// <summary>
        /// Creates resource detail and inserts it into the manifest.
        /// </summary>
        /// <param name="resources">The resources.</param>
        /// <param name="manifest">The manifest.</param>
        internal override void AddToManifest(Collection<VirtualResource> resources, SystemManifest manifest)
        {
            foreach (VirtualResource resource in resources)
            {
                EventLogCollectorDetail detail = manifest.Resources.GetResource<EventLogCollectorDetail>(resource.VirtualResourceId);
                if (detail == null)
                {
                    detail = CreateDetail(resource);
                    manifest.Resources.Add(detail);
                }
            }
        }

        public override ResourceDetailBase CreateBaseDetail(VirtualResource resource) => CreateDetail(resource);

        private EventLogCollectorDetail CreateDetail(VirtualResource resource)
        {
            EventLogCollector collector = resource as EventLogCollector;

            return new EventLogCollectorDetail
            {
                ResourceId = collector.VirtualResourceId,
                ResourceType = EnumUtil.Parse<VirtualResourceType>(collector.ResourceType),
                Name = collector.Name,
                Description = collector.Description,
                InstanceCount = collector.InstanceCount,
                Platform = collector.Platform,
                Enabled = collector.Enabled,
                HostName = collector.HostName,
                ComponentsData = collector.ComponentsData,
                EntryTypesData = collector.EntryTypesData,
                PollingInterval = collector.PollingInterval
            };
        }
    }
}