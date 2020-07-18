using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Xml;
using System;
using System.IO;
using System.Linq;

namespace HP.ScalableTest.Framework.Automation
{
    internal class CitrixPublishedApplicationHandler : VirtualResourceHandler
    {
        private readonly CitrixWorkerDetail _worker = null;
        private readonly OfficeWorkerCredential _credential = null;
        private readonly string _citrixServer = string.Empty;


        /// <summary>
        /// Initializes a new instance of the <see cref="MachineReservationHandler"/> class.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        public CitrixPublishedApplicationHandler(SystemManifest manifest)
            : base(manifest)
        {
            _worker = SystemManifest.Resources.OfType<CitrixWorkerDetail>().First();
            _citrixServer = _worker.ServerHostname;

            _credential = SystemManifest.Resources.Credentials.First();

            VirtualResourceEventBus.OnStartMainRun += VirtualResourceEventBus_OnStartMainRun;
            VirtualResourceEventBus.OnShutdownResource += VirtualResourceEventBus_OnShutdownResource;
            VirtualResourceEventBus.OnPauseResource += VirtualResourceEventBus_OnPauseResource;
            VirtualResourceEventBus.OnResumeResource += VirtualResourceEventBus_OnResumeResource;
        }

        void VirtualResourceEventBus_OnStartMainRun(object sender, VirtualResourceEventBusRunArgs e)
        {
            TraceFactory.Logger.Debug("Attempting to start the Citrix Published Application");

            // Now the actual Citrix published app must be started.
            ChangeMachineStatusMessage("Starting Published App");
            CitrixSessionManager.StartPublishedApp(_credential, _citrixServer, _worker.PublishedApp);

            ChangeResourceState(RuntimeState.Running);
        }

        private void VirtualResourceEventBus_OnResumeResource(object sender, EventArgs e)
        {
            ChangeResourceState(RuntimeState.Completed);
        }

        private void VirtualResourceEventBus_OnPauseResource(object sender, EventArgs e)
        {
            ChangeResourceState(RuntimeState.Paused);
        }

        private void VirtualResourceEventBus_OnShutdownResource(object sender, EventArgs e)
        {
            TraceFactory.Logger.Info("Shutdown Session for {0}".FormatWith(_credential.UserName));
            try
            {
                ChangeMachineStatusMessage("Logoff Citrix");

                CitrixSessionManager.ResetCitrixSession(_credential.UserName, _citrixServer);
                CitrixSessionManager.RemoveFromAdminGroup(_credential, _citrixServer);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error ending session for user {0}".FormatWith(_credential.UserName), ex);
            }

            ChangeResourceState(RuntimeState.Offline);
        }

        /// <summary>
        /// Creates all EventLogCollector virtual resources.
        /// </summary>
        public override void Start()
        {
            ChangeResourceState(RuntimeState.Starting);
            
            OpenManagementServiceEndpoint(_credential.UserName);

            ChangeMachineStatusMessage("Configuring User");
            CitrixSessionManager.ConfigureLocalUserGroups(_credential, _citrixServer);

            ChangeMachineStatusMessage("Resetting Citrix");
            CitrixSessionManager.ResetCitrixSession(_credential.UserName, _citrixServer);

            // Register and let the dispatcher know it's available to run.
            SessionProxyBackendConnection.RegisterResource(ServiceEndpoint);
        }
    }
}
