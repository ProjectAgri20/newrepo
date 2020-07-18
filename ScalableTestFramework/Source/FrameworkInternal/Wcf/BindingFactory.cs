using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace HP.ScalableTest.Framework.Wcf
{
    /// <summary>
    /// Creates bindings used for WCF communication.
    /// </summary>
    public static class BindingFactory
    {
        /// <summary>
        /// Creates a <see cref="Binding" /> for the specified <see cref="MessageTransferType" />.
        /// </summary>
        /// <param name="messageTransferType">The <see cref="MessageTransferType" /> to create a binding for.</param>
        /// <returns>A <see cref="Binding" /> for the specified <see cref="MessageTransferType" />.</returns>
        public static Binding CreateBinding(MessageTransferType messageTransferType)
        {
            return CreateBinding(messageTransferType, TimeSpan.FromMinutes(10));
        }

        /// <summary>
        /// Creates a <see cref="Binding" /> for the specified <see cref="MessageTransferType" /> with a configurable timeout.
        /// </summary>
        /// <param name="messageTransferType">The <see cref="MessageTransferType" /> to create a binding for.</param>
        /// <param name="timeout">The timeout value for service send/receive operations.</param>
        /// <returns>A <see cref="Binding" /> for the specified <see cref="MessageTransferType" />.</returns>
        public static Binding CreateBinding(MessageTransferType messageTransferType, TimeSpan timeout)
        {
            Binding binding;
            switch (messageTransferType)
            {
                case MessageTransferType.Http:
                    binding = CreateHttpBinding();
                    break;

                case MessageTransferType.CompressedHttp:
                    binding = CreateCompressionBinding();
                    break;

                case MessageTransferType.NamedPipe:
                    binding = CreateNamedPipeBinding();
                    break;

                default:
                    throw new ArgumentException($"Unsupported MessageTransferType {messageTransferType}.", nameof(messageTransferType));
            }

            // The only pertinent timeout to set is SendTimeout - this governs the amount of time allowed
            // for round-trip communication, including sending the request, processing, and waiting for the response.
            binding.SendTimeout = timeout;
            return binding;
        }

        private static Binding CreateHttpBinding()
        {
            return new WSHttpBinding(SecurityMode.None)
            {
                BypassProxyOnLocal = true,
                UseDefaultWebProxy = false,
                MaxReceivedMessageSize = 512 * 1024,
                ReaderQuotas = XmlDictionaryReaderQuotas.Max
            };
        }

        private static Binding CreateCompressionBinding()
        {
            BinaryMessageEncodingBindingElement messageEncoding = new BinaryMessageEncodingBindingElement
            {
                CompressionFormat = CompressionFormat.GZip,
                ReaderQuotas = XmlDictionaryReaderQuotas.Max
            };

            HttpTransportBindingElement httpTransport = new HttpTransportBindingElement()
            {
                BypassProxyOnLocal = true,
                UseDefaultWebProxy = false,
                MaxReceivedMessageSize = int.MaxValue,
                TransferMode = TransferMode.Streamed
            };

            return new CustomBinding(messageEncoding, httpTransport);
        }

        private static Binding CreateNamedPipeBinding()
        {
            return new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
            {
                MaxReceivedMessageSize = 1024 * 1024 * 20,
                ReaderQuotas = XmlDictionaryReaderQuotas.Max
            };
        }
    }
}
