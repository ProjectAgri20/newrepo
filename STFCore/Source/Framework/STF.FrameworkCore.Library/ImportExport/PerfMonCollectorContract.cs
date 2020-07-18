using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract (used for import/export) for PerfMonCollector.
    /// </summary>
    [DataContract(Name = "PerfMonCollector", Namespace = "")]
    [ObjectFactory(VirtualResourceType.PerfMonCollector)]
    public class PerfMonCollectorContract : ResourceContract
    {
        /// <summary>
        /// Loads the PerfMonCollectorContract from the specified VirtualResource object.
        /// </summary>
        /// <param name="resource"></param>
        public override void Load(VirtualResource resource)
        {
            base.Load(resource);

            var worker = resource as PerfMonCollector;

            HostName = worker.HostName;
        }

        /// <summary>
        /// The host name of the target server.
        /// </summary>
        [DataMember]
        public string HostName { get; set; }
    }
}
