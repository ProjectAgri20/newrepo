using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Contains static parameters used to host and connect to the lock service.
    /// </summary>
    public static class LockServiceEndpoint
    {
        /// <summary>
        /// The port on which the lock service is hosted.
        /// </summary>
        public static readonly int Port = 12777;

        /// <summary>
        /// The service name used in the lock service URI.
        /// </summary>
        public static readonly string ServiceName = "LockService";

        /// <summary>
        /// The <see cref="Framework.Wcf.MessageTransferType" /> used for the lock service.
        /// </summary>
        /// <remarks>
        /// Do not change - the WCF dual channel client requires a simple HTTP binding.
        /// </remarks>
        public static readonly MessageTransferType MessageTransferType = MessageTransferType.Http;

        /// <summary>
        /// Builds the URI to connect to the <see cref="ILockService" /> at the specified address.
        /// </summary>
        /// <param name="address">The address where the lock service is hosted.</param>
        /// <returns>A <see cref="Uri" /> for connecting to the lock service.</returns>
        public static Uri BuildUri(string address)
        {
            UriBuilder builder = new UriBuilder(Uri.UriSchemeHttp, address, Port, ServiceName);
            return builder.Uri;
        }
    }
}
