using System.ServiceModel;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.Lock;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Service.Lock
{
    public class LockWindowsService : SelfInstallingServiceBase
    {
        private ServiceHost _lockService;

        public LockWindowsService()
            : base("LockService", "STF Lock Service")
        {
            Description = "STF service for implementing locks across the enterprise system.";
        }

        /// <summary>
        /// Starts this service instance.
        /// </summary>
        /// <param name="args">The <see cref="CommandLineArguments" /> provided to the start command.</param>
        protected override void StartService(CommandLineArguments args)
        {
            TraceFactory.Logger.Debug("Lock Service endpoint - Opening");

            _lockService = new WcfHost<ILockService>(new LockService(), LockServiceEndpoint.MessageTransferType, LockServiceEndpoint.BuildUri("localhost"));
            _lockService.Open();

            TraceFactory.Logger.Debug("Lock Service endpoint - Opened");
        }

        protected override void StopService()
        {
            _lockService.Close();
        }
    }
}
