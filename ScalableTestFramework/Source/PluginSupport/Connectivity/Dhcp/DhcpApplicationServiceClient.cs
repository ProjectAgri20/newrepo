using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.PluginSupport.Connectivity.Dhcp
{
    /// <summary>
    /// Client class for <see cref="IDhcpApplication"/>
    /// </summary>
    public sealed class DhcpApplicationServiceClient : WcfClient<IDhcpApplication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpApplicationServiceClient"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        private DhcpApplicationServiceClient(Uri endpoint)
            : base(MessageTransferType.Http, endpoint)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DhcpApplicationServiceClient"/>
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        /// <returns></returns>
        public static DhcpApplicationServiceClient Create(string serviceHost)
        {
            return new DhcpApplicationServiceClient(BuildUri(serviceHost));
        }

        public static Uri BuildUri(string serviceHost)
        {
            return new Uri("http://{0}:{1}/{2}".FormatWith(serviceHost, 13017, "DhcpService"));
        }
    }
}
