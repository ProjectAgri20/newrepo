using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Supports session map activities specific to the AdminWorker resource
    /// </summary>
    [ObjectFactory(VirtualResourceType.CitrixWorker)]
    public class CitrixWorkerInstance : WorkerInstanceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorkerInstance"/> class.
        /// </summary>
        /// <param name="instanceId">A unique id for this instance.</param>
        /// <param name="detail">The detail information from the <see cref="SystemManifest" />.</param>
        public CitrixWorkerInstance(string instanceId, ResourceDetailBase detail)
            : base(instanceId, detail)
        {
        }
    }
}
