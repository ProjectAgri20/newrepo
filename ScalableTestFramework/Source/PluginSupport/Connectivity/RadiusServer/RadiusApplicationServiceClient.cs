using System;

using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.PluginSupport.Connectivity.RadiusServer
{
    public sealed class RadiusApplicationServiceClient : WcfClient<IRadiusApplication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadiusApplicationServiceClient"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        private RadiusApplicationServiceClient(Uri endpoint)
            : base(MessageTransferType.Http, endpoint)
        {
        }

        /// <summary>
        /// Creates a new <see cref="RadiusApplicationServiceClient"/>
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        /// <returns></returns>
        public static RadiusApplicationServiceClient Create(string serviceHost)
        {
            return new RadiusApplicationServiceClient(BuildUri(serviceHost));
        }

        public static Uri BuildUri(string serviceHost)
        {
            return new Uri("http://{0}:{1}/{2}".FormatWith(serviceHost, 13022, "RadiusService"));
        }
    }
}
