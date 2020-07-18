using System;
using System.ServiceModel;
using HP.ScalableTest.Framework.Settings;
using System.ServiceModel.Channels;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Client class for <see cref="IClientControllerService"/>
    /// </summary>
    public sealed class ClientControllerServiceConnection : WcfClient<IClientControllerService>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientControllerServiceConnection"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="transferType"></param>
        private ClientControllerServiceConnection(Uri endpoint, MessageTransferType transferType)
            : base(transferType, endpoint)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ClientControllerServiceConnection"/>.
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        /// <returns></returns>
        public static ClientControllerServiceConnection Create(string serviceHost)
        {
            Uri uri = null;
            MessageTransferType transferType;

            if (GlobalSettings.IsDistributedSystem)
            {
                uri = WcfService.ClientController.GetHttpUri(serviceHost);
                transferType = MessageTransferType.CompressedHttp;
            }
            else
            {
                uri = WcfService.ClientController.GetLocalNamedPipeUri();
                transferType = MessageTransferType.NamedPipe;
            }

            return new ClientControllerServiceConnection(uri, transferType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ServiceHost CreateServiceHost(IClientControllerService obj)
        {
            Uri uri = null;
            MessageTransferType transType;

            if (GlobalSettings.IsDistributedSystem)
            {
                uri = WcfService.ClientController.GetLocalHttpUri();
                transType = MessageTransferType.CompressedHttp;
            }
            else
            {
                uri = WcfService.ClientController.GetLocalNamedPipeUri();
                transType = MessageTransferType.NamedPipe;
            }

            TraceFactory.Logger.Debug("Creating {0}".FormatWith(uri.AbsoluteUri));
            return new WcfHost<IClientControllerService>(obj, transType, uri);
        }
    }
}
