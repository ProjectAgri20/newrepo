using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Wcf;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Class VirtualResourceManagementConnection. This class cannot be inherited.
    /// </summary>
    public sealed class VirtualResourceManagementConnection : WcfClient<IVirtualResourceManagementService>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualResourceManagementConnection"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="transferType">the transfer protocol</param>
        private VirtualResourceManagementConnection(Uri endpoint, MessageTransferType transferType)
            : base(transferType, endpoint)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="EndpointNotFoundException"></exception>
        public static VirtualResourceManagementConnection Create(Uri endpoint)
        {
            MessageTransferType transferType;
            if (endpoint.AbsoluteUri.StartsWith("http"))
            {
                transferType = MessageTransferType.CompressedHttp;
            }
            else if (endpoint.AbsoluteUri.StartsWith("net.pipe"))
            {
                transferType = MessageTransferType.NamedPipe;
            }
            else
            {
                throw new EndpointNotFoundException("Unsupported endpoint: {0}".FormatWith(endpoint.AbsoluteUri));
            }

            return new VirtualResourceManagementConnection(endpoint, transferType);
        }

        /// <summary>
        /// Creates a new <see cref="VirtualResourceManagementConnection"/>.
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        /// <param name="port">the port number</param>
        /// <returns></returns>
        public static VirtualResourceManagementConnection Create(string serviceHost, int port)
        {
            Uri uri = null;
            if (GlobalSettings.IsDistributedSystem)
            {
                uri = WcfService.VirtualResource.GetHttpUri(serviceHost, port);
            }
            else
            {
                uri = WcfService.VirtualResource.GetLocalNamedPipeUri(port.ToString());
            }

            return Create(uri);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ServiceHost CreateServiceHost(string hostname, string key)
        {
            Uri uri = null;

            uri = WcfService.VirtualResource.GetLocalNamedPipeUri(key);
            var transType = MessageTransferType.NamedPipe;

            TraceFactory.Logger.Debug("Creating {0}".FormatWith(uri.AbsoluteUri));
            return new WcfHost<IVirtualResourceManagementService>(typeof(VirtualResourceManagementService), transType, uri);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ServiceHost CreateServiceHost(string hostname, int port)
        {
            Uri uri = null;

            uri = WcfService.VirtualResource.GetHttpUri(hostname, port);
            var transType = MessageTransferType.CompressedHttp;

            TraceFactory.Logger.Debug("Creating {0}".FormatWith(uri.AbsoluteUri));
            return new WcfHost<IVirtualResourceManagementService>(typeof(VirtualResourceManagementService), transType, uri);
        }
    }
}

