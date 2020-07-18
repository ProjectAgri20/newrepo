using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Class providing different connectivity testing functionalities
    /// </summary>
    public sealed class ConnectivityUtilityServiceClient : WcfClient<IConnectivityUtility>
    {
        public static string ServiceName = "ConnectivityUtilityService";
        public static int ServicePort = 13024;

        private ConnectivityUtilityServiceClient(Uri endpoint)
            : base(MessageTransferType.Http, endpoint)
        {
        }

        /// <summary>
        /// Create service endpoint for accessing Connectivity service operation contracts
        /// </summary>
        /// <param name="serviceHost">IP Address of the machine where the service is hosted</param>
        /// <returns></returns>
        public static ConnectivityUtilityServiceClient Create(string serviceHost)
        {
            return new ConnectivityUtilityServiceClient(BuildUri(serviceHost));
        }

        public static Uri BuildUri(string serviceHost)
        {
            return new Uri($"http://{serviceHost}:{ServicePort}/{ServiceName}");
        }
    }
}
