using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.PluginSupport.Hpcr
{
    /// <summary>
    /// Client class for the <see cref="IHpcrExecutionProxyService" />.
    /// </summary>
    public sealed class HpcrExecutionProxyClient : WcfClient<IHpcrExecutionProxyService>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HpcrExecutionProxyClient" /> class.
        /// </summary>
        /// <param name="address">The service address.</param>
        public HpcrExecutionProxyClient(string address)
            : this(HpcrProxyUri.BuildUri(address))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HpcrExecutionProxyClient" /> class.
        /// </summary>
        /// <param name="endpoint">The service endpoint.</param>
        public HpcrExecutionProxyClient(Uri endpoint)
            : base(MessageTransferType.CompressedHttp, endpoint)
        {
        }
    }
}
