using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml;

namespace HP.ScalableTest.Framework.Wcf
{
    /// <summary>
    /// A base dual-channel WCF client.
    /// </summary>
    /// <typeparam name="TChannel">The contract for this WCF client.</typeparam>
    /// <typeparam name="TCallback">The contract for the WCF callback.</typeparam>
    /// <remarks>
    /// This class is an alternative to the DuplexClient provided by WCF.  It uses a traditional
    /// ServiceHost endpoint for the callback channel and allows for a more descriptive callback address.
    /// </remarks>
    public class WcfDualChannelClient<TChannel, TCallback> : WcfClient<TChannel>, IDisposable
        where TChannel : class
        where TCallback : class
    {
        private const int _port = 10101;
        private WcfHost<TCallback> _callbackHost;

        /// <summary>
        /// Gets the callback endpoint for this WCF dual-channel client.
        /// </summary>
        public Uri CallbackEndpoint { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfDualChannelClient{TChannel, TCallback}"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint at which the service is hosted.</param>
        /// <param name="callback">The <typeparamref name="TCallback" /> object to use for the callback.</param>
        /// <param name="callbackIdentifier">An identifier to use in the callback URI.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="endpoint" /> is null.
        /// <para>or</para>
        /// <paramref name="callback" /> is null.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads", Justification = "False positive.")]
        public WcfDualChannelClient(string endpoint, TCallback callback, string callbackIdentifier)
            : this(new Uri(endpoint), callback, callbackIdentifier)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfDualChannelClient{TChannel, TCallback}"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint at which the service is hosted.</param>
        /// <param name="callback">The <typeparamref name="TCallback" /> object to use for the callback.</param>
        /// <param name="callbackIdentifier">An identifier to use in the callback URI.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="endpoint" /> is null.
        /// <para>or</para>
        /// <paramref name="callback" /> is null.
        /// </exception>
        public WcfDualChannelClient(Uri endpoint, TCallback callback, string callbackIdentifier)
            : base(MessageTransferType.Http, endpoint)
        {
            CallbackEndpoint = BuildCallbackUri(endpoint, callbackIdentifier);
            _callbackHost = OpenCallbackHost(callback, CallbackEndpoint);

            Endpoint.Behaviors.Add(new DualChannelBehavior(CallbackEndpoint));
        }

        private static Uri BuildCallbackUri(Uri serviceEndpoint, string callbackIdentifier)
        {
            string host = serviceEndpoint.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ? "localhost" : Dns.GetHostEntry("127.0.0.1").HostName;
            UriBuilder uriBuilder = new UriBuilder(Uri.UriSchemeHttp, host, _port, $"{Guid.NewGuid()}-{callbackIdentifier}");
            return uriBuilder.Uri;
        }

        private static WcfHost<TCallback> OpenCallbackHost(TCallback callbackObject, Uri callbackEndpoint)
        {
            WcfHost<TCallback> callbackChannel = null;
            try
            {
                callbackChannel = new WcfHost<TCallback>(callbackObject, MessageTransferType.Http, callbackEndpoint);
                callbackChannel.Open();
                return callbackChannel;
            }
            catch
            {
                try
                {
                    (callbackChannel as IDisposable)?.Dispose();
                }
                catch (CommunicationObjectFaultedException)
                {
                    callbackChannel.Abort();
                }
                throw;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    _callbackHost?.Close();
                }
                catch (CommunicationObjectFaultedException)
                {
                    _callbackHost.Abort();
                }

                try
                {
                    Close();
                }
                catch (CommunicationObjectFaultedException)
                {
                    Abort();
                }
            }
        }

        #endregion

        #region Helper Classes

        private sealed class DualChannelBehavior : IEndpointBehavior
        {
            private readonly Uri _replyTo;

            public DualChannelBehavior(Uri replyTo)
            {
                _replyTo = replyTo;
            }

            public void Validate(ServiceEndpoint endpoint)
            {
            }

            public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
            {
            }

            public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
            {
            }

            public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
            {
                if (clientRuntime == null)
                {
                    throw new ArgumentNullException(nameof(clientRuntime));
                }

                var inspector = new DualChannelInspector(_replyTo);
                clientRuntime.MessageInspectors.Add(inspector);
            }
        }

        private sealed class DualChannelInspector : IClientMessageInspector
        {
            private readonly Uri _replyTo;

            public DualChannelInspector(Uri replyTo)
            {
                _replyTo = replyTo;
            }

            public void AfterReceiveReply(ref Message reply, object correlationState)
            {
            }

            public object BeforeSendRequest(ref Message request, IClientChannel channel)
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                request.Headers.ReplyTo = new EndpointAddress(_replyTo);
                request.Headers.MessageId = new UniqueId(Guid.NewGuid());
                return null;
            }
        }

        #endregion
    }
}
