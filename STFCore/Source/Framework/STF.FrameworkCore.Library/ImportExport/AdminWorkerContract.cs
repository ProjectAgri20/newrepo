using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using System.Runtime.Serialization;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Contract for Admin Worker import/export operations.
    /// </summary>
    [DataContract(Name="AdminWorker", Namespace="")]
    [ObjectFactory(VirtualResourceType.AdminWorker)]
    public class AdminWorkerContract : OfficeWorkerContract
    {
        /// <summary>
        /// Loads the AdminWorkerContract from the specified VirtualResource object.
        /// </summary>
        /// <param name="resource"></param>
        public override void Load(VirtualResource resource)
        {
            base.Load(resource);

            var worker = resource as AdminWorker;

            ExecutionMode = worker.ExecutionMode;
        }

        // TODO: Remove hidden member and rework serialization at later date
        /// <summary>
        /// Execution Mode
        /// </summary>
        [DataMember]
        public new ExecutionMode ExecutionMode { get; set; }
    }
}
