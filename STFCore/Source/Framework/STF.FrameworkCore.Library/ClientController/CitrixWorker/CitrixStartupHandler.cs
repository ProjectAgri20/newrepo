using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using System.Linq;

namespace HP.ScalableTest.Framework.Automation
{
    [VirtualResourceHandler(VirtualResourceType.CitrixWorker)]
    internal class CitrixStartupHandler : VirtualResourceHandler
    {
        private readonly SystemManifest _manifest = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CitrixStartupHandler" /> class.
        /// </summary>
        /// <remarks>
        /// Specifically avoid cascading this constructor to the base class that
        /// passes the manifest because it will try to start the Client Controller
        /// endpoint, which will then cause it be started twice.
        /// </remarks>
        /// <param name="manifest"></param>
        public CitrixStartupHandler(SystemManifest manifest)
        {
            _manifest = manifest;
        }

        #region IVirtualResourceHandler

        public override void Start()
        {
            IVirtualResourceHandler handler = null;

            // Determine if this is a standard Citrix Worker or if it is 
            // a published app that needs to run.
            var detail = _manifest.Resources.OfType<CitrixWorkerDetail>().First();

            switch (detail.WorkerRunMode)
            {
                case CitrixWorkerRunMode.None:
                    TraceFactory.Logger.Debug("Starting a Citrix published app");
                    handler = new CitrixPublishedApplicationHandler(_manifest);
                    break;
                default:
                    TraceFactory.Logger.Debug("Starting a Citrix Worker");
                    handler = new CitrixWorkerHandler(_manifest);
                    break;
            }

            handler.Start();
        }

        #endregion
    }
}
