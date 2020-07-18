using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Framework.DartLog
{
    /// <summary>
    /// Client for Dart Log Collector Service
    /// </summary>
    public class DartLogCollectorClient : IDartLogCollectorService
    {
        string ServiceLocation;

        /// <summary>
        /// Pulls info from GlobalSettings "DartServiceCollection"
        /// </summary>
        public DartLogCollectorClient()
        {
            ServiceLocation = GlobalSettings.Items["DartServiceLocation"];
        }
        
        /// <summary>
        /// Calls Service Host to collect Logs
        /// </summary>
        /// <param name="device"></param>
        /// <param name="sessionID"></param>
        /// <param name="email"></param>
        public void CollectLog(string device, string sessionID, string email)
        {
            using (var client = new WcfClient<IDartLogCollectorService>(MessageTransferType.Http, WcfService.CollectDartLogService.GetHttpUri(ServiceLocation)))
            {
                client.Channel.CollectLog(device, sessionID, email);
            }
        }

        /// <summary>
        /// Calls Service Host to Flush the Memory Buffer at Specified IP
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public bool Flush(string IP)
        {
            using (var client = new WcfClient<IDartLogCollectorService>(MessageTransferType.Http, WcfService.CollectDartLogService.GetHttpUri(ServiceLocation)))
            {
                return client.Channel.Flush(IP);
            }
        }

        /// <summary>
        /// Starts the dart card to collect at the specified IP address
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public bool Start(string IP)
        {
            using (var client = new WcfClient<IDartLogCollectorService>(MessageTransferType.Http, WcfService.CollectDartLogService.GetHttpUri(ServiceLocation)))
            {
                return client.Channel.Start(IP);
            }
        }

        /// <summary>
        /// Stops Collection of Dart logs at the specified IP address.
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public bool Stop(string IP)
        {
            using (var client = new WcfClient<IDartLogCollectorService>(MessageTransferType.Http, WcfService.CollectDartLogService.GetHttpUri(ServiceLocation)))
            {
                return client.Channel.Stop(IP);
            }
        }
    }
}
