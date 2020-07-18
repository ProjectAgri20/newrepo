using System;
using System.ServiceModel;
using System.Threading.Tasks;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Service.Citrix
{
    /// <summary>
    /// Windows service that hosts the Citrix Queue Monitor service.
    /// </summary>
    public class CitrixQueueMonitorWindowsService : SelfInstallingServiceBase
    {
        private ServiceHost _citrixQueueMonitorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CitrixQueueMonitorWindowsService"/> class.
        /// </summary>
        public CitrixQueueMonitorWindowsService()
            : base("CitrixQueueMonitorService", "STF Citrix Queue Monitor")
        {
            Description = "STF service for monitoring Citrix print queues.";
        }

        /// <summary>
        /// Starts this service instance.
        /// </summary>
        /// <param name="args">The <see cref="CommandLineArguments" /> provided to the start command.</param>
        protected override void StartService(CommandLineArguments args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            // Load the settings, either from the command line or the local cache
            FrameworkServiceHelper.LoadSettings(args);

            Task.Factory.StartNew(() =>
            {
                TraceFactory.Logger.Debug("Update server info");
                FrameworkServiceHelper.UpdateServerInfo();
                TraceFactory.Logger.Debug("Update server info - Complete");
            });
            // Load and start the citrix queue monitor service
            _citrixQueueMonitorService = new WcfHost<ICitrixQueueMonitorService>(
                typeof(CitrixQueueMonitorService),
                MessageTransferType.Http,
                WcfService.CitrixQueueMonitor.GetLocalHttpUri()
            );

            _citrixQueueMonitorService.Open();
        }

        /// <summary>
        /// Stops this service instance.
        /// </summary>
        protected override void StopService()
        {
            if (_citrixQueueMonitorService.State != CommunicationState.Closed)
            {
                _citrixQueueMonitorService.Close();
            }
        }

    }
}
