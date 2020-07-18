using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.PluginSupport.Hpcr
{
    /// <summary>
    /// Client class for the <see cref="IHpcrConfigurationProxyService" />.
    /// </summary>
    public sealed class HpcrConfigurationProxyClient : WcfClient<IHpcrConfigurationProxyService>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HpcrConfigurationProxyClient" /> class.
        /// </summary>
        /// <param name="address">The service address.</param>
        public HpcrConfigurationProxyClient(string address)
            : this(HpcrProxyUri.BuildUri(address))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HpcrConfigurationProxyClient" /> class.
        /// </summary>
        /// <param name="endpoint">The service endpoint.</param>
        public HpcrConfigurationProxyClient(Uri endpoint)
            : base(MessageTransferType.CompressedHttp, endpoint)
        {
        }
    }
}
