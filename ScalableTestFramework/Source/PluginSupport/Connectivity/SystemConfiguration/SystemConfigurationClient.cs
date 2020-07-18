using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.PluginSupport.Connectivity.SystemConfiguration
{
    /// <summary>
    /// Client class for <see cref="ISystemConfiguration"/>
    /// </summary>
    public sealed class SystemConfigurationClient : WcfClient<ISystemConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConfigurationClient"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        private SystemConfigurationClient(Uri endpoint)
            : base(MessageTransferType.Http, endpoint)
        {
        }

        /// <summary>
        /// Creates a new <see cref="SystemConfigurationClient"/>
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        /// <returns></returns>
        public static SystemConfigurationClient Create(string serviceHost)
        {
            return new SystemConfigurationClient(BuildUri(serviceHost));
        }

        public static Uri BuildUri(string serviceHost)
        {
            return new Uri("http://{0}:{1}/{2}".FormatWith(serviceHost, 13023, "SystemConfigurationService"));
        }
    }
}
