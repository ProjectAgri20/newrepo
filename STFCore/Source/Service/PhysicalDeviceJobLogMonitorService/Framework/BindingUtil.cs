using System;
using System.Net;
using System.ServiceModel.Channels;

namespace HP.Epr.WebServicesResponder
{
    public static class BindingUtil
    {
        /// <summary>
        /// Creates a binding compatible with Jedi web services (very basic binding, with HTTPS support).
        /// </summary>
        /// <param name="uri">The URI that will be connected to.</param>
        /// <returns></returns>
        public static Binding CreateBinding(Uri uri)
        {
            TextMessageEncodingBindingElement encoding = new TextMessageEncodingBindingElement();
            encoding.MessageVersion = MessageVersion.Soap12WSAddressingAugust2004;
            var quota = new System.Xml.XmlDictionaryReaderQuotas();
//						quota.MaxNameTableCharCount = 1 << 20;
// ADD MYs adds until the ending comment
                        quota.MaxNameTableCharCount = int.MaxValue;
                        quota.MaxDepth = int.MaxValue;
                        quota.MaxNameTableCharCount = int.MaxValue;
                        quota.MaxBytesPerRead = int.MaxValue;
                        quota.MaxArrayLength = int.MaxValue;
                        quota.MaxStringContentLength = int.MaxValue;
// To here!

            encoding.ReaderQuotas = quota;

            //HTTPS with Basic auth is the default option for accessing FutureSmart web services
            //HTTP is supported when the "Allow a Non-Secure Connection for Web Services" setting on the EWS's
            //Troubleshooting tab is enabled
            HttpTransportBindingElement httpsTransport;
            if (uri.Scheme == "http")
            {
                httpsTransport = new HttpTransportBindingElement();
            }
            else
            {
                httpsTransport = new HttpsTransportBindingElement();
                httpsTransport.AuthenticationScheme = AuthenticationSchemes.Basic;
            }

            //Tell WCF what resource we're accessing
            System.ServiceModel.Channels.Binding wsBinding;
            wsBinding = new CustomBinding(encoding, httpsTransport);
//            httpsTransport.MaxReceivedMessageSize *= 100;
                        httpsTransport.MaxReceivedMessageSize = int.MaxValue;
                        
                        //httpsTransport.HostNameComparisonMode = HostNameComparisonMode.Exact;

            return wsBinding;
        }
    }
}
