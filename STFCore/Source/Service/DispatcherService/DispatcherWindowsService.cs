using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Service.Dispatcher
{
    /// <summary>
    /// Dispatcher service.
    /// </summary>
    public class DispatcherWindowsService : SelfInstallingServiceBase
    {
        private ServiceHost _sessionManagementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatcherWindowsService"/> class.
        /// </summary>
        public DispatcherWindowsService()
            : base("DispatcherService", "STF Dispatcher Service")
        {
            Description = "STF Dispatcher Service";
        }

        /// <summary>
        /// Starts this service instance.
        /// </summary>
        /// <param name="args">The <see cref="CommandLineArguments" /> provided to the start command.</param>
        protected override void StartService(CommandLineArguments args)
        {
            Thread.CurrentThread.SetName("Main");

            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            TraceFactory.Logger.Debug("Load settings");
            // Load the settings, either from the command line or the local cache
            FrameworkServiceHelper.LoadSettings(args, autoRefresh: true);
            TraceFactory.Logger.Debug("Load settings - Complete");

            Task.Factory.StartNew(() =>
            {
                TraceFactory.Logger.Debug("Updating Server Info");
                FrameworkServiceHelper.UpdateServerInfo();
                TraceFactory.Logger.Debug("Updating Server Info - Complete");
            });

            Task.Factory.StartNew(() =>
            {
                _sessionManagementService = new WcfHost<ISessionDispatcher>
                (
                    typeof(SessionDispatcher),
                    MessageTransferType.CompressedHttp,
                    WcfService.SessionServer.GetLocalHttpUri()
                );
                TraceFactory.Logger.Debug("Starting Session Management Service");
                _sessionManagementService.Open();
                TraceFactory.Logger.Debug("Starting Session Management Service - Complete");
            });
        }

        /// <summary>
        /// Stops this service instance.
        /// </summary>
        protected override void StopService()
        {
            Task.Factory.StartNew(() =>
                {
                    if (_sessionManagementService.State != CommunicationState.Closed)
                    {
                        _sessionManagementService.Close();
                    }
                });
        }

    }
}
