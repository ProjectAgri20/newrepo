using System;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Network.Wcf;
using System.Collections.Generic;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Client connection class for <see cref="ISessionProxyBackend"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Backend")]
    public sealed class SessionProxyBackendClient : WcfClient<ISessionProxyBackend>
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="SessionProxyBackendClient"/> class from being created.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        private SessionProxyBackendClient(Uri endpoint)
            : base(WcfBinding.CreateCompressionBinding(), endpoint)
        {
        }

        /// <summary>
        /// Defines the <see cref="Uri"/> for this client connection.
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <returns></returns>
        public static Uri ServiceUri(string serviceHost, string sessionId)
        {
            return new Uri("http://{0}:{1}/SessionBackend-{2}".FormatWith
                (
                    serviceHost,
                    (int)WcfService.SessionBackend,
                    sessionId,
                    WcfBinding.CreateCompressionBinding()
                ));
        }

        /// <summary>
        /// Creates a new <see cref="SessionProxyBackendClient" />.
        /// </summary>
        /// <param name="serviceHost">Hostname to connect to.</param>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <returns></returns>
        public static SessionProxyBackendClient Create(string serviceHost, string sessionId)
        {
            return new SessionProxyBackendClient(ServiceUri(serviceHost, sessionId));
        }

        /// <summary>
        /// Runs the specified <see cref="ISessionProxyBackend"/> action against the backend proxy service.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void Run(Action<ISessionProxyBackend> action)
        {
            var retryAction = new Action(() =>
            {
                using (var proxy = SessionProxyBackendClient.Create(GlobalDataStore.Manifest.Dispatcher, GlobalDataStore.Manifest.SessionId))
                {
                    action(proxy.Channel);
                }
            });

            try
            {
                RetryUtility.RetryAction(retryAction, 2, TimeSpan.FromSeconds(1), new List<Type>() { typeof(Exception) });
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Unable to communicate with Session Proxy backend: {0}".FormatWith(ex.Message));
            }
        }
    }
}
