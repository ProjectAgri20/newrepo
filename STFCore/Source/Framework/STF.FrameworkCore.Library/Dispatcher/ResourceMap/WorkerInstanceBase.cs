using System.Linq;
using HP.ScalableTest.Framework.Manifest;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Supports session map activities specific to the AdminWorker resource
    /// </summary>
    public class WorkerInstanceBase : ResourceInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerInstanceBase"/> class.
        /// </summary>
        /// <param name="name">The name of this instance.</param>
        /// <param name="detail">The detail information from the <see cref="SystemManifest" />.</param>
        public WorkerInstanceBase(string name, ResourceDetailBase detail)
            : base(name, detail)
        {
        }

        public override void Validate(ParallelLoopState loopState, SystemManifest manifest, string machineName)
        {
            TraceFactory.Logger.Debug("Validating activities for {0} on Machine {1}".FormatWith(Id, machineName));
            Parallel.ForEach<ResourceMetadata>(Metadata, (m, l) => m.Validate(l, Id, manifest, machineName));
            if (SessionMapElement.AnyElementsSetTo<ResourceMetadata>(Metadata, RuntimeState.Error))
            {
                var elements = SessionMapElement.GetElements<ResourceMetadata>(RuntimeState.Error, Metadata).Select(x => x.Detail.Name);
                TraceFactory.Logger.Debug("Metadata in ERROR: {0}".FormatWith(string.Join(", ", elements.ToArray())));

                TraceFactory.Logger.Debug("Activity validation failed for {0}".FormatWith(Id));
                MapElement.UpdateStatus("Activity validation failed", RuntimeState.Error);
                loopState.Break();
                return;
            }
            else if (SessionMapElement.AnyElementsSetTo<ResourceMetadata>(Metadata, RuntimeState.Warning))
            {
                var elements = SessionMapElement.GetElements<ResourceMetadata>(RuntimeState.Warning, Metadata).Select(x => x.Detail.Name);
                TraceFactory.Logger.Debug("Metadata in WARNING: {0}".FormatWith(string.Join(", ", elements.ToArray())));

                TraceFactory.Logger.Debug("Activity validation caused a warning for {0}".FormatWith(Id));
                MapElement.UpdateStatus("Activity validation caused a warning", RuntimeState.Warning);
                return;
            }
            MapElement.UpdateStatus("Validated", RuntimeState.Validated);
        }
    }
}
