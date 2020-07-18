using System;

namespace HP.ScalableTest.PluginSupport.Hpcr
{
    /// <summary>
    /// Contains methods for building URIs to connect to the HPCR proxy services.
    /// </summary>
    public static class HpcrProxyUri
    {
        /// <summary>
        /// The port at which the HPCR proxy services are hosted.
        /// </summary>
        public static readonly int Port = 12676;

        /// <summary>
        /// The service name used in the URI address for the HPCR proxy service.
        /// </summary>
        public static readonly string ServiceName = "HpcrProxy";

        /// <summary>
        /// The URI for an HPCR proxy service residing on the local machine.
        /// </summary>
        public static readonly Uri LocalHost = BuildUri("localhost");

        /// <summary>
        /// Builds the URI to connect to the HPCR proxy service at the specified address.
        /// </summary>
        /// <param name="address">The service address or hostname.</param>
        /// <returns></returns>
        public static Uri BuildUri(string address)
        {
            UriBuilder builder = new UriBuilder("http", address, Port, ServiceName);
            return builder.Uri;
        }
    }
}
