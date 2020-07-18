using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Framework.Monitor
{
    /// <summary>
    /// Client connection to the STFMonitor service.
    /// </summary>
    public sealed class STFMonitorServiceConnection : WcfClient<ISTFMonitorService>
    {
        private STFMonitorServiceConnection(Uri endpoint)
            : base(MessageTransferType.Http, endpoint)
        {
        }

        /// <summary>
        /// Create a client connection to the STFMonitor service.
        /// </summary>
        /// <param name="serviceHostName"></param>
        /// <returns>A new instance of the <see cref="STFMonitorServiceConnection"/> class.</returns>
        public static STFMonitorServiceConnection Create(string serviceHostName)
        {
            Uri endpoint = new Uri($"http://{serviceHostName}:{(int)WcfService.STFMonitor}/{WcfService.STFMonitor}");

            return new STFMonitorServiceConnection(endpoint);
        }
    }
}
