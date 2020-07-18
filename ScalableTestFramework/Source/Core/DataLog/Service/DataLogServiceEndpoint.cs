using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Core.DataLog.Service
{
    /// <summary>
    /// Contains static parameters used to host and connect to the data log service.
    /// </summary>
    public static class DataLogServiceEndpoint
    {
        /// <summary>
        /// The port on which the data log service is hosted.
        /// </summary>
        public static readonly int Port = 12678;

        /// <summary>
        /// The service name used in the data log service URI.
        /// </summary>
        public static readonly string ServiceName = "DataLogService";

        /// <summary>
        /// The <see cref="Framework.Wcf.MessageTransferType" /> used for the data log service.
        /// </summary>
        public static readonly MessageTransferType MessageTransferType = MessageTransferType.CompressedHttp;

        /// <summary>
        /// Builds the URI to connect to the <see cref="IDataLogService" /> at the specified address.
        /// </summary>
        /// <param name="address">The address where the data log service is hosted.</param>
        /// <returns>A <see cref="Uri" /> for connecting to the data log service.</returns>
        public static Uri BuildUri(string address)
        {
            UriBuilder builder = new UriBuilder(Uri.UriSchemeHttp, address, Port, ServiceName);
            return builder.Uri;
        }
    }
}
