using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Supports session map activities specific to the AdminWorker resource
    /// </summary>
    [ObjectFactory(VirtualResourceType.AdminWorker)]
    public class AdminWorkerInstance : WorkerInstanceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminWorkerInstance"/> class.
        /// </summary>
        /// <param name="instanceId">A unique id for this instance.</param>
        /// <param name="detail">The detail information from the <see cref="SystemManifest" />.</param>
        public AdminWorkerInstance(string instanceId, ResourceDetailBase detail)
            : base(instanceId, detail)
        {
        }

        /// <summary>
        /// Starts the setup process for this instance.
        /// </summary>
        public override void StartSetup()
        {
            // Determine if there are any activities in the pre run collection and
            // if so proceed to execute them.  If not just return.
            if (Detail.MetadataDetails.Any(e => e.Plan != null && e.Plan.Phase == ResourceExecutionPhase.Setup))
            {
                TraceFactory.Logger.Debug("Executing setup activities");
                PerformAction(c => c.StartSetup());
            }
        }

        /// <summary>
        /// Starts the teardown process for this instance.
        /// </summary>
        public override void StartTeardown()
        {            
            // Determine if there are any activities in the post run collection and
            // if so proceed to execute them.  If not just return.
            if (Detail.MetadataDetails.Any(e => e.Plan != null && e.Plan.Phase == ResourceExecutionPhase.Teardown))
            {
                TraceFactory.Logger.Debug("Executing teardown activities");
                PerformAction(c => c.StartTeardown());
            }
        }
    }
}
