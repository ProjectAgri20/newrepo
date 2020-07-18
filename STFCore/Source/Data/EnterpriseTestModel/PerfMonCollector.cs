using System;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    [ObjectFactory(VirtualResourceType.PerfMonCollector)]
    public partial class PerfMonCollector
    {
        public PerfMonCollector()
            : this("PerfMonCollector")
        {
        }

        public PerfMonCollector(string resourceType)
            : base(resourceType)
        {
            HostName = string.Empty;
        }

        protected override string GetExecutionPlan(ResourceMetadataDetail detail)
        {
            return string.Empty;
        }

        protected override void LoadChildDetail(ResourceDetailBase detail)
        {
            var resourceDetail = detail as PerfMonCollectorDetail;
            HostName = resourceDetail.HostName;
        }

        public override void CopyResourceProperties(VirtualResource resource)
        {
            base.CopyResourceProperties(resource);
            PerfMonCollector collector = resource as PerfMonCollector;
            if (collector != null)
            {
                HostName = collector.HostName;
            }
            else
            {
                throw new ArgumentException("Resource must be of type PerfMonCollector.", "resource");
            }
        }
    }
}
