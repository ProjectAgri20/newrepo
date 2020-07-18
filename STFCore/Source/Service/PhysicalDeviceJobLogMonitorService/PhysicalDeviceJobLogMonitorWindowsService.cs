using System;
using System.ServiceModel;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    public class PhysicalDeviceJobLogMonitorWindowsService : SelfInstallingServiceBase
    {
        private ServiceHost _serviceHost;
        private PhysicalDeviceJobLogMonitorService _service;

        public PhysicalDeviceJobLogMonitorWindowsService()
            : base("PhysicalDeviceJobLogMonitorService", "STF Physical Device Job Log Monitor Service")
        {
            Description = "STF service for monitoring physical device logs";
        }

        /// <summary>
        /// Starts this service instance.
        /// </summary>
        /// <param name="args">The <see cref="CommandLineArguments" /> provided to the start command.</param>
        protected override void StartService(CommandLineArguments args)
        {
            try
            {
                // Load settings from the database
                FrameworkServiceHelper.LoadSettings(args, true);

                _service = new PhysicalDeviceJobLogMonitorService();
                StartService();
            }
            catch(Exception ex)
            {
                TraceFactory.Logger.Error("Error starting services", ex);
            }
        }

        private void StartService()
        {
            TraceFactory.Logger.Debug("Starting Physical Device Job Log Monitor Service");
            try
            {
                var serviceAddress = WcfService.PhysicalDeviceJobLogMonitorService.GetLocalHttpUri();
                TraceFactory.Logger.Info("Starting service at address {0}".FormatWith(serviceAddress));
                _serviceHost = new WcfHost<IPhysicalDeviceJobLogMonitorService>
                    (
                        _service,
                        MessageTransferType.CompressedHttp,
                        serviceAddress
                    );
                TraceFactory.Logger.Debug("Starting Physical Device Job Log Monitor Service");
                _serviceHost.Open();
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error: ", ex);
            }
        }

        /// <summary>
        /// Stops this service instance.
        /// </summary>
        protected override void StopService()
        {
            try
            {
                if (_serviceHost != null && _serviceHost.State != CommunicationState.Closed)
                {
                    _serviceHost.Close();
                }
                TraceFactory.Logger.Info("Stopped subscription service");
            }
            catch(Exception ex)
            {
                TraceFactory.Logger.Error("Stopping publish and subscription services", ex);
            }
        }
    }
}

