using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data contract for Citrix Worker (used for import/export).
    /// </summary>
    [DataContract(Name = "CitrixWorker", Namespace = "")]
    [ObjectFactory(VirtualResourceType.CitrixWorker)]
    public class CitrixWorkerContract : OfficeWorkerContract
    {
        /// <summary>
        /// Loads the CitrixWorkerContract from the specified VirtualResource object.
        /// </summary>
        /// <param name="resource"></param>
        public override void Load(VirtualResource resource)
        {
            base.Load(resource);

            var worker = resource as CitrixWorker;

            DBWorkerRunMode = worker.DBWorkerRunMode;
            ServerHostname = worker.ServerHostname;
            PublishedApp = worker.PublishedApp;
        }

        /// <summary>
        /// Worker Run Mode
        /// </summary>
        [DataMember]
        public string DBWorkerRunMode { get; set; }

        /// <summary>
        /// Citrix Server Host Name
        /// </summary>
        [DataMember]
        public string ServerHostname { get; set; }

        /// <summary>
        /// The Published App Name
        /// </summary>
        [DataMember]
        public string PublishedApp { get; set; }
    }
}
