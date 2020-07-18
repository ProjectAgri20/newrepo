using System.Collections.ObjectModel;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used to build detail information on Citrix Workers for the system manifest.
    /// </summary>
    internal class CitrixWorkerDetailBuilder : OfficeWorkerDetailBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CitrixWorkerDetailBuilder"/> class.
        /// </summary>
        /// <param name="config">The builder.</param>
        /// <param name="resourcePacker">The resource packer.</param>
        public CitrixWorkerDetailBuilder(SystemManifestAgent config, VirtualResourcePacker resourcePacker)
            : base(config, resourcePacker, VirtualResourceType.OfficeWorker)
        {
        }

        internal override void AddToManifest(Collection<VirtualResource> resources, SystemManifest manifest)
        {
            LoadResources<CitrixWorkerDetail>(resources, manifest);
        }

        public override OfficeWorkerDetail CreateDetail(VirtualResource resource)
        {
            CitrixWorker worker = resource as CitrixWorker;

            CitrixWorkerDetail detail = new CitrixWorkerDetail();
            CreateBaseWorkerDetail(worker, detail);

            detail.PublishedApp = worker.PublishedApp;
            detail.WorkerRunMode = worker.RunMode;
            detail.ServerHostname = worker.ServerHostname;

            CreateMetadataDetail(resource, detail);
            return detail;
        }
    }
}