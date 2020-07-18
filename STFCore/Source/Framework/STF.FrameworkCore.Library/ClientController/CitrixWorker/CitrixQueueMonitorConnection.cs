using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Client class for <see cref="ICitrixQueueMonitorService"/>
    /// </summary>
    public sealed class CitrixQueueMonitorConnection : WcfClient<ICitrixQueueMonitorService>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CitrixQueueMonitorConnection"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        private CitrixQueueMonitorConnection(Uri endpoint)
            : base(MessageTransferType.Http, endpoint)
        {
        }

        /// <summary>
        /// Creates a new <see cref="CitrixQueueMonitorConnection"/>.
        /// </summary>
        /// <param name="serviceHost">Hostname to connect to.</param>
        /// <returns></returns>
        public static CitrixQueueMonitorConnection Create(string serviceHost)
        {
            var endpoint = new Uri("http://{0}:{1}/{2}".FormatWith(serviceHost, (int)WcfService.CitrixQueueMonitor, WcfService.CitrixQueueMonitor));

            return new CitrixQueueMonitorConnection(endpoint);
        }
    }
}
