using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.PluginSupport.Connectivity.PacketCapture
{
    /// <summary>
    /// Client class for <see cref="IPacketCapture"/>
    /// </summary>
    public sealed class PacketCaptureServiceClient : WcfClient<IPacketCapture>
    {
        public static string ServiceName = "PacketCaptureService";
        public static int ServicePort = 13016;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketCaptureServiceClient"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        private PacketCaptureServiceClient(Uri endpoint)
            : base(MessageTransferType.Http, endpoint)
        {
        }

        /// <summary>
        /// Creates a new <see cref="PacketCaptureServiceClient"/>
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        /// <returns></returns>
        public static PacketCaptureServiceClient Create(string serviceHost)
        {
            return new PacketCaptureServiceClient(BuildUri(serviceHost));
        }

        public static Uri BuildUri(string serviceHost)
        {
            return new Uri($"http://{serviceHost}:{ServicePort}/{ServiceName}");
        }
    }
}
