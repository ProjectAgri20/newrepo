using System;
using System.ServiceModel;
using System.Threading.Tasks;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Service.Monitor
{
    /// <summary>
    /// Windows service that monitors STF resources in the environment.
    /// </summary>
    public class STFMonitorWindowsService : SelfInstallingServiceBase
    {
        private STFMonitorService _monitorService = null;
        private ServiceHost _serviceEndpoint = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="STFMonitorWindowsService"/> class.
        /// </summary>
        public STFMonitorWindowsService()
            : base("STFMonitorService", "STF Monitor Service")
        {
            this.Description = "STF service hosting monitor processes";
        }

        /// <summary>
        /// Starts this service instance.
        /// </summary>
        /// <param name="args">The <see cref="CommandLineArguments" /> provided to the start command.</param>
        /// <remarks>
        /// Valid startup args are:
        /// 0: Database Host Name where the SystemSettings database resides.
        /// 1: [Optional] Session Expiration Interval in hours.
        /// </remarks>
        protected override void StartService(CommandLineArguments args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            FrameworkServiceHelper.LoadSettings(args);

            Task.Factory.StartNew(() =>
            {
                FrameworkServiceHelper.UpdateServerInfo();
                TraceFactory.Logger.Debug("Updating Server Info - Complete");
            });

            int expirationInterval = args.GetParameterValue<int>("expirationInterval");

            //Start the service
            _monitorService = new STFMonitorService(expirationInterval);
            _monitorService.Start();

            //Open the server WCF endpoint
            _serviceEndpoint = new WcfHost<ISTFMonitorService>
            (
                _monitorService,
                MessageTransferType.Http,
                WcfService.STFMonitor.GetLocalHttpUri()
            );

            _serviceEndpoint.Open();

        }

        /// <summary>
        /// Stops this service instance.
        /// </summary>
        protected override void StopService()
        {
            //Close the endpoint first
            _serviceEndpoint.Close();
            _serviceEndpoint = null;

            _monitorService.Stop();
            _monitorService = null;            
        }
    }
}
