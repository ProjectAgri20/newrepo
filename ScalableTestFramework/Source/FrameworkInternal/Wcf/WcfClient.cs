using System;
using System.ServiceModel;

namespace HP.ScalableTest.Framework.Wcf
{
    /// <summary>
    /// A base WCF client.
    /// </summary>
    /// <typeparam name="T">The contract for this WCF client.</typeparam>
    public class WcfClient<T> : ClientBase<T> where T : class
    {
        /// <summary>
        /// Gets the interface channel for this WCF client.
        /// </summary>
        public new T Channel => base.Channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfClient{T}" /> class.
        /// </summary>
        /// <param name="messageTransferType">The <see cref="MessageTransferType" /> to use for WCF communication.</param>
        /// <param name="endpoint">The endpoint at which the service is hosted.</param>
        /// <exception cref="ArgumentNullException"><paramref name="endpoint" /> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Justification = "False positive.")]
        public WcfClient(MessageTransferType messageTransferType, string endpoint)
            : this(messageTransferType, new Uri(endpoint))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfClient{T}" /> class.
        /// </summary>
        /// <param name="messageTransferType">The <see cref="MessageTransferType" /> to use for WCF communication.</param>
        /// <param name="endpoint">The endpoint at which the service is hosted.</param>
        /// <exception cref="ArgumentNullException"><paramref name="endpoint" /> is null.</exception>
        public WcfClient(MessageTransferType messageTransferType, Uri endpoint)
            : base(BindingFactory.CreateBinding(messageTransferType), new EndpointAddress(endpoint))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfClient{T}" /> class with a configurable timeout.
        /// </summary>
        /// <param name="messageTransferType">The <see cref="MessageTransferType" /> to use for WCF communication.</param>
        /// <param name="endpoint">The endpoint at which the service is hosted.</param>
        /// <param name="timeout">The timeout value for service send/receive operations.</param>
        /// <exception cref="ArgumentNullException"><paramref name="endpoint" /> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Justification = "False positive.")]
        public WcfClient(MessageTransferType messageTransferType, string endpoint, TimeSpan timeout)
            : this(messageTransferType, new Uri(endpoint), timeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfClient{T}" /> class with a configurable timeout.
        /// </summary>
        /// <param name="messageTransferType">The <see cref="MessageTransferType" /> to use for WCF communication.</param>
        /// <param name="endpoint">The endpoint at which the service is hosted.</param>
        /// <param name="timeout">The timeout value for service send/receive operations.</param>
        /// <exception cref="ArgumentNullException"><paramref name="endpoint" /> is null.</exception>
        public WcfClient(MessageTransferType messageTransferType, Uri endpoint, TimeSpan timeout)
            : base(BindingFactory.CreateBinding(messageTransferType, timeout), new EndpointAddress(endpoint))
        {
        }
    }
}
