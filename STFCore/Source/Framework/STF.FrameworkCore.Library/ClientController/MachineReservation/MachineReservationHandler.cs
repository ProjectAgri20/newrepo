using System;
using System.Linq;
using System.Threading;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Handles the creation and state changes for an Machine Reservation
    /// </summary>
    [VirtualResourceHandler(VirtualResourceType.MachineReservation)]
    internal class MachineReservationHandler : VirtualResourceHandler
    {
        private readonly MachineReservationDetail _reservation = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineReservationHandler"/> class.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        public MachineReservationHandler(SystemManifest manifest)
            : base(manifest)
        {
            _reservation = GlobalDataStore.Manifest.Resources.OfType<MachineReservationDetail>().First();

            VirtualResourceEventBus.OnStartMainRun += VirtualResourceEventBus_OnStartMainRun;
            VirtualResourceEventBus.OnShutdownResource += VirtualResourceEventBus_OnShutdownResource;
            VirtualResourceEventBus.OnPauseResource += VirtualResourceEventBus_OnPauseResource;
            VirtualResourceEventBus.OnResumeResource += VirtualResourceEventBus_OnResumeResource;
        }

        void VirtualResourceEventBus_OnStartMainRun(object sender, VirtualResourceEventBusRunArgs e)
        {
            TraceFactory.Logger.Debug("Returning a completed signal as the machine is now up and running");

            // There's a bit of a timing problem here.  If the completed is sent back too quickly the
            // state will be changed before the run handler has a chance to set the state to running.
            // So delay for a few seconds so that this update doesn't beat the session proxy Run
            // Handler's update.  Otherwise the state will be left in Running.
            Thread.Sleep(5000);
            ChangeResourceState(RuntimeState.Completed);
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
            ChangeResourceState(RuntimeState.Offline);
        }

        /// <summary>
        /// Creates the resources.
        /// </summary>
        public override void Start()
        {
            ChangeResourceState(RuntimeState.Starting);
            
            OpenManagementServiceEndpoint(_reservation.Name);

            // Register and let the dispatcher know it's available to run.
            SessionProxyBackendConnection.RegisterResource(ServiceEndpoint);
        }
    }
}