using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Supports session map activities specific to the OfficeWorker resource
    /// </summary>
    [ObjectFactory(VirtualResourceType.OfficeWorker)]
    public class OfficeWorkerInstance : WorkerInstanceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorkerInstance"/> class.
        /// </summary>
        /// <param name="instanceId">A unique id for this instance.</param>
        /// <param name="detail">The detail information from the <see cref="SystemManifest" />.</param>
        public OfficeWorkerInstance(string instanceId, ResourceDetailBase detail)
            : base(instanceId, detail)
        {
        }
    }
}
