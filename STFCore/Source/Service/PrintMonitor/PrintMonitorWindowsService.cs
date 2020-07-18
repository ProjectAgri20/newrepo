using System;
using System.ServiceModel;
using System.Threading.Tasks;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Service.PrintMonitor
{
    /// <summary>
    /// Windows service that hosts the Print Monitor service.
    /// </summary>
    public class PrintMonitorWindowsService : SelfInstallingServiceBase
    {
        private ServiceHost _printMonitorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintMonitorWindowsService"/> class.
        /// </summary>
        public PrintMonitorWindowsService()
            : base("PrintMonitorService", "STF Print Monitor")
        {
            Description = "STF service for monitoring print jobs.";
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
                TraceFactory.Logger.Debug("Updating Server Info");
                FrameworkServiceHelper.UpdateServerInfo();
                TraceFactory.Logger.Debug("Updating Server Info - Complete");
            });
            // Load and start the print monitor service
            _printMonitorService = new WcfHost<IPrintMonitorService>(
                typeof(PrintMonitorService),
                MessageTransferType.Http,
                WcfService.PrintMonitor.GetLocalHttpUri());

            _printMonitorService.Open();
            PrintMonitorService.Instance.Start();
        }

        /// <summary>
        /// Stops this service instance.
        /// </summary>
        protected override void StopService()
        {
            if (_printMonitorService.State != CommunicationState.Closed)
            {
                _printMonitorService.Close();
            }
        }

    }
}
