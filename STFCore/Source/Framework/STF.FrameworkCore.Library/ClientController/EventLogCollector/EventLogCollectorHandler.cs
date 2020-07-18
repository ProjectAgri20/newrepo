using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.ClientController.EventLogCollector;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Handles the creation and state changes for an EventLogCollector Virtual Resource
    /// </summary>
    [VirtualResourceHandler(VirtualResourceType.EventLogCollector)]
    internal class EventLogCollectorHandler : VirtualResourceHandler
    {
        private EventLogCollector _collector = null;

        private enum EventLogName
        {
            Application,
            System,
            Security
        }

        /// <summary>
        /// Creates a new EventLogCollectorHandler instance.
        /// </summary>
        /// <param name="manifest"></param>
        public EventLogCollectorHandler(SystemManifest manifest)
            : base(manifest)
        {
        }

        /// <summary>
        /// Creates all EventLogCollector virtual resources.
        /// </summary>
        public override void Start()
        {
            LoadManifest();

            ChangeResourceState(Runtime.RuntimeState.Starting);

            // Sign up for the event that is fired when the client tells us to run.
            VirtualResourceEventBus.OnStartMainRun += VirtualResourceEventBus_OnStart;
            VirtualResourceEventBus.OnShutdownResource += VirtualResourceEventBus_OnStop;
            VirtualResourceEventBus.OnPauseResource += VirtualResourceEventBus_OnPause;
            VirtualResourceEventBus.OnResumeResource += VirtualResourceEventBus_OnResume;
            
            OpenManagementServiceEndpoint(_collector.ResourceDefinition.HostName);
            
            TraceFactory.Logger.Info("Monitoring: {0}{1}{2}".FormatWith(_collector.ResourceDefinition.HostName, Environment.NewLine, DisplayCollection(_collector.ResourceDefinition.Components)));

            SessionProxyBackendConnection.RegisterResource(ServiceEndpoint);

            TraceFactory.Logger.Debug("Dispatcher notified that system is ready to start");
        }

        private void LoadManifest()
        {
            _collector = new EventLogCollector(SystemManifest.SessionId, SystemManifest.Resources.OfType<EventLogCollectorDetail>().FirstOrDefault());
        }

        void VirtualResourceEventBus_OnStart(object sender, VirtualResourceEventBusRunArgs e)
        {
            _collector.Start();

            ChangeResourceState(RuntimeState.Running);
        }

        /// <summary>
        /// Event raised by the client factory when the "Shutdown" call has been made.
        /// </summary>
        private void VirtualResourceEventBus_OnStop(object sender, EventArgs e)
        {
            ChangeResourceState(RuntimeState.ShuttingDown);
            _collector.Stop();
            ChangeResourceState(RuntimeState.Offline);
        }

        private void VirtualResourceEventBus_OnPause(object sender, EventArgs e)
        {
            TraceFactory.Logger.Debug("Pause message received.");
            ChangeResourceState(RuntimeState.Paused);
        }

        private void VirtualResourceEventBus_OnResume(object sender, EventArgs e)
        {
            TraceFactory.Logger.Debug("Resume message received.");
        }


        private static string DisplayCollection(Collection<string> collection)
        {
            StringBuilder result = new StringBuilder();

            foreach (string item in collection)
            {
                result.Append(item).Append(Environment.NewLine);
            }

            return result.ToString();
        }
    }
}