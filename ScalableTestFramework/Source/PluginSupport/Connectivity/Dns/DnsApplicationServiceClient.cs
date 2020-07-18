using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.PluginSupport.Connectivity.DnsApp
{
    /// <summary>
    /// Client class for <see cref="IDnsApplication"/>
    /// </summary>
    public sealed class DnsApplicationServiceClient : WcfClient<IDnsApplication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DnsApplicationServiceClient"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        private DnsApplicationServiceClient(Uri endpoint)
            : base(MessageTransferType.Http, endpoint)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DnsApplicationServiceClient"/>
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        /// <returns></returns>
        public static DnsApplicationServiceClient Create(string serviceHost)
        {
            return new DnsApplicationServiceClient(BuildUri(serviceHost));
        }

        public static Uri BuildUri(string serviceHost)
        {
            return new Uri("http://{0}:{1}/{2}".FormatWith(serviceHost, 13018, "DnsService"));
        }
    }
}
