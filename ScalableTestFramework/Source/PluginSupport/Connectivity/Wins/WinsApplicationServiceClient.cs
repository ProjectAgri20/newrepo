using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.PluginSupport.Connectivity.Wins
{
    /// <summary>
    /// Client class for <see cref="IWinsApplication"/>
    /// </summary>
    public sealed class WinsApplicationServiceClient : WcfClient<IWinsApplication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinsApplicationServiceClient"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        private WinsApplicationServiceClient(Uri endpoint)
            : base(MessageTransferType.Http, endpoint)
        {
        }

        /// <summary>
        /// Creates a new <see cref="WinsApplicationServiceClient"/>
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        /// <returns></returns>
        public static WinsApplicationServiceClient Create(string serviceHost)
        {
            return new WinsApplicationServiceClient(BuildUri(serviceHost));
        }

        public static Uri BuildUri(string serviceHost)
        {
            return new Uri("http://{0}:{1}/{2}".FormatWith(serviceHost, 13019, "WinsService"));
        }
    }
}
