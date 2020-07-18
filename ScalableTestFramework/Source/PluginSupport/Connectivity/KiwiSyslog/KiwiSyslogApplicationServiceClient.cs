using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.PluginSupport.Connectivity.KiwiSyslog
{
    /// <summary>
    /// Client class for <see cref="IKiwiSyslogApplication"/>
    /// </summary>
    public sealed class KiwiSyslogApplicationServiceClient : WcfClient<IKiwiSyslogApplication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhcpApplicationServiceClient"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        private KiwiSyslogApplicationServiceClient(Uri endpoint)
            : base(MessageTransferType.Http, endpoint)
        {
        }

        /// <summary>
        /// Creates a new <see cref="KiwiSyslogApplicationServiceClient"/>
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        /// <returns></returns>
        public static KiwiSyslogApplicationServiceClient Create(string serviceHost)
        {
            return new KiwiSyslogApplicationServiceClient(BuildUri(serviceHost));
        }

        public static Uri BuildUri(string serviceHost)
        {
            return new Uri("http://{0}:{1}/{2}".FormatWith(serviceHost, 13021, "SyslogService"));
        }
    }
}
