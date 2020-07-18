using System;

namespace HP.ScalableTest.Plugin.FlashFirmware.BashLog
{
    /// <summary>
    /// Defines the names and port numbers of WCF service endpoints.
    /// </summary>
    public enum WcfService
    {
        /// <summary>
        /// Indicates no service.
        /// </summary>
        None = 0,

        /// <summary>
        /// Session Management service
        /// </summary>
        SessionServer = 12660,

        /// <summary>
        /// Session Backend service
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Backend")]
        SessionBackend = 12661,

        /// <summary>
        /// Client Resource service
        /// </summary>
        ClientController = 12662,

        /// <summary>
        /// Virtual Resource management service
        /// </summary>
        VirtualResource = 12663,

        /// <summary>
        /// The lock used for any global critical sections, which is enterprise wide.
        /// </summary>
        GlobalLock = 12664,

        /// <summary>
        /// The lock used for any local critical sections, which is machine level.
        /// </summary>
        LocalLock = 12665,

        /// <summary>
        /// Print Monitor service
        /// </summary>
        PrintMonitor = 12666,

        /// <summary>
        /// The session proxy program that hosts a session
        /// </summary>
        SessionProxy = 12667,

        /// <summary>
        /// Data Gateway service
        /// </summary>
        DataGateway = 12668,

        /// <summary>
        /// Used by proxy processes to notify the dispatcher that they are started.
        /// </summary>
        SessionProxyCallback = 12669,

        /// <summary>
        /// Test Document Library service
        /// </summary>
        TestDocument = 12671,

        /// <summary>
        /// Client callback for session management service
        /// </summary>
        SessionClient = 12672,

        /// <summary>
        /// Asset Inventory for Production
        /// </summary>
        AssetInventory = 12673,

        /// <summary>
        /// Asset Inventory for Beta
        /// </summary>
        AssetInventoryBeta = 12674,

        /// <summary>
        /// Asset Inventory for Dev
        /// </summary>
        AssetInventoryDev = 12675,

        /// <summary>
        /// ePrint Job Monitor Service
        /// </summary>
        EPrintJobMonitor = 12677,

		/// <summary>
		/// Data log service
		/// </summary>
		DataLog = 12678,

        /// <summary>
        /// Citrix Queue Monitor
        /// </summary>
        CitrixQueueMonitor = 12680,

        /// <summary>
        /// The Endpoint Responder Proxy Service for Dev, Beta and Prod
        /// </summary>
        EndpointResponderProxy = 13014,

        /// <summary>
        /// The STF Monitor service
        /// </summary>
        STFMonitor = 13015,

        /// <summary>
        /// Physical device job log monitor service
        /// </summary>
        PhysicalDeviceJobLogMonitorService = 13020,

        /// <summary>
        /// Callback for lock services
        /// </summary>
        /// <remarks>
        /// Note that this port number is in a different area to avoid
        /// conflict with the ones defined above. Be sure not to assign
        /// any other ports between 10632-10782 as the  number of lock 
        /// clients can grow up to 150.
        /// </remarks>
        LockClient = 10632,

        /// <summary>
        /// Collect Event Log Service
        /// </summary>
        CollectDartLogService = 13025,

        /// <summary>
        /// Sends commands between handler and .exe workers
        /// </summary>
        SequentialWorkerService = 13026,

        /// <summary>
        /// Collects Bash Logs
        /// </summary>
        BashLogcollectorService = 13027
    }

    /// <summary>
    /// Static class to provide extension methods for <see cref="WcfService"/>.
    /// </summary>
    public static class WcfServiceHelper
    {
        /// <summary>
        /// Gets the standard URI for this service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="hostName">The machine where the service is hosted.</param>
        /// <returns></returns>
        public static Uri GetHttpUri(this WcfService service, string hostName)
        {
            return GetHttpUri(service, hostName, (int)service);
        }

        /// <summary>
        /// Gets the standard URI for this service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="hostName">The machine where the service is hosted.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public static Uri GetHttpUri(this WcfService service, string hostName, int port)
        {
            return new Uri(string.Format("http://{0}:{1}/{2}",hostName, port, service));
        }

        /// <summary>
        /// Gets the standard URI for this service
        /// </summary>
        /// <param name="service"></param>
        /// <param name="hostName"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static Uri GetHttpUri(this WcfService service, string hostName, string extension)
        {
            return GetHttpUri(service, hostName, (int)service, extension);
        }

        /// <summary>
        /// Gets the standard URI for this service
        /// </summary>
        /// <param name="service"></param>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static Uri GetHttpUri(this WcfService service, string hostName, int port, string extension)
        {
            return new Uri(string.Format("http://{0}:{1}/{2}-{3}",hostName, port, service, extension));
        }

        /// <summary>
        /// Gets the self-hosted URI for this service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public static Uri GetLocalHttpUri(this WcfService service)
        {
            return new Uri(string.Format("http://localhost:{0}/{1}",(int)service, service));
        }

        /// <summary>
        /// Gets the self-hosted URI for this service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        public static Uri GetLocalHttpUri(this WcfService service, string extension)
        {
            return new Uri(string.Format("http://localhost:{0}/{1}-{2}",(int)service, service, extension));
        }

        /// <summary>
        /// Gets the local named pipe URI.
        /// </summary>
        /// <param name="service">The service name to be used in the URI.</param>
        /// <returns>The URI that described the named pipe.</returns>
        public static Uri GetLocalNamedPipeUri(this WcfService service)
        {
            return new Uri(string.Format("net.pipe://localhost/{0}",service));
        }

        /// <summary>
        /// Gets the Named Piped Uri
        /// </summary>
        /// <param name="service"></param>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static Uri GetNamedPipeUri(this WcfService service, string hostName)
        {
            return new Uri(string.Format("net.pipe://{0}/{1}",hostName, service));
        }

        /// <summary>
        /// Get the local Named Piped Uri
        /// </summary>
        /// <param name="service"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static Uri GetLocalNamedPipeUri(this WcfService service, string extension)
        {
            return new Uri(string.Format("net.pipe://localhost/{0}-{1}",service, extension));
        }
    }


}
