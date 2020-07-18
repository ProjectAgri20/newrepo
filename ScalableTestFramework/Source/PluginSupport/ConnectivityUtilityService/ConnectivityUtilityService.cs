using System.ServiceModel;
using System.ServiceProcess;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.PluginSupport.ConnectivityUtilityService
{
    internal class ConnectivityUtilityService : ServiceBase
    {
        public ServiceHost _connectivityUtilityServiceHost = null;

        public ConnectivityUtilityService()
        {
            ServiceName = "ConnectivityUtilityService";
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            Logger.LogDebug("Starting Connectivity Utility Service");

            // Create service host for Service Contract
            _connectivityUtilityServiceHost = new WcfHost<IConnectivityUtility>(typeof(ConnectivityUtility), MessageTransferType.Http, ConnectivityUtilityServiceClient.BuildUri("localhost"));
            _connectivityUtilityServiceHost.Open();
        }

        protected override void OnStop()
        {
            base.OnStop();

            Logger.LogDebug("Stopping Connectivity Utility Service");

            if (_connectivityUtilityServiceHost.State != CommunicationState.Closed)
            {
                _connectivityUtilityServiceHost.Close();
            }
        }
    }
}
