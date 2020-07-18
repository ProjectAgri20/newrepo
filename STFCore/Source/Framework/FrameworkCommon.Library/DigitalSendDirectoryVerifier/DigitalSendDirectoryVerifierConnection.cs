using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Framework.DigitalSendDirectoryVerifier
{
    public sealed class DigitalSendDirectoryVerifierConnection : WcfClient<IDigitalSendDirectoryVerifierService>
    {
        private DigitalSendDirectoryVerifierConnection(Uri endpoint)
            : base(MessageTransferType.CompressedHttp, endpoint)
        {

        }

        public static DigitalSendDirectoryVerifierConnection Create(string serviceHost)
        {
            var endpoint = new Uri("http://{0}:{1}/{2}".FormatWith(serviceHost, (int)WcfService.DigitalSendDirectoryVerifier, WcfService.DigitalSendDirectoryVerifier));

            return new DigitalSendDirectoryVerifierConnection(endpoint);
        }
    }
}
