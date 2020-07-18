using System;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    [ObjectFactory(VirtualResourceType.EventLogCollector)]
    public partial class EventLogCollector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogCollector"/> class.
        /// </summary>
        public EventLogCollector()
            : this("EventLogCollector")
        {
        }

        public EventLogCollector(string resourceType)
            : base(resourceType)
        {
            HostName = string.Empty;
            ComponentsData = string.Empty;
            PollingInterval = 5;
        }

        protected override void LoadChildDetail(ResourceDetailBase detail)
        {
            var resourceDetail = detail as EventLogCollectorDetail;

            HostName = resourceDetail.HostName;
            ComponentsData = resourceDetail.ComponentsData;
            EntryTypesData = resourceDetail.EntryTypesData;
            PollingInterval = resourceDetail.PollingInterval;
        }

        /// <summary>
        /// Copies all relevant metadata from the target VirtualResource to this instance.
        /// </summary>
        /// <param name="resource">The resource to copy from.</param>
        public override void CopyResourceProperties(VirtualResource resource)
        {
            base.CopyResourceProperties(resource);
            EventLogCollector collector = resource as EventLogCollector;
            if (collector != null)
            {
                HostName = collector.HostName;
                ComponentsData = collector.ComponentsData;
                EntryTypesData = collector.EntryTypesData;
                PollingInterval = collector.PollingInterval;
            }
            else
            {
                throw new ArgumentException("Resource must be of type EventLogCollector.", "resource");
            }
        }
    }
}
