using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract (used for import/export) for EventLogCollector.
    /// </summary>
    [DataContract(Name = "LoadTester", Namespace = "")]
    [ObjectFactory(VirtualResourceType.LoadTester)]
    public class LoadTesterContract : ResourceContract
    {
        /// <summary>
        /// Loads the LoadTesterContract from the specified VirtualResource object.
        /// </summary>
        /// <param name="resource"></param>
        public override void Load(VirtualResource resource)
        {
            base.Load(resource);

            var worker = resource as LoadTester;

            ThreadsPerVM = worker.ThreadsPerVM;
        }

        /// <summary>
        /// Defines how many worker threads should run per VM.
        /// </summary>
        [DataMember]
        public int ThreadsPerVM { get; set; }
    }
}
