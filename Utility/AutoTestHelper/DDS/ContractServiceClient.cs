using System;
using System.Collections.Specialized;
using System.Configuration;
using HP.ScalableTest.Framework.Wcf;

namespace HP.RDL.EDT.ContractDDS
{
    public class ContractServiceClient : WcfClient<IContractService>
    {
        private static string _dsDevelopment;
        private static string _dsProduction;
        

        /// <summary>
        /// Utilizes the WCF binding to create the end points for access to the service.
        /// </summary>
        protected ContractServiceClient(Uri endPoint)
            : base(MessageTransferType.Http, endPoint)
        {
            NameValueCollection systems = ConfigurationManager.GetSection("DDS") as NameValueCollection;

            _dsDevelopment = systems["Development"];
            _dsProduction = systems["Production"];
           
        }

        /// <summary>
        /// Starts the process for generating a channel to the service.
        /// </summary>
        /// <param name="serviceHost">string</param>
        /// <returns>ContractServiceClient</returns>
        public static ContractServiceClient Create(string serviceHost)
        {
            return new ContractServiceClient(GetUri(serviceHost));
        }

        /// <summary>
        /// Returns a formatted URI for access to the service on the given address.
        /// </summary>
        /// <param name="address">address</param>
        /// <returns>Uri</returns>
        public static Uri GetUri(string address)
        {
            if (string.IsNullOrEmpty(_dsProduction) || string.IsNullOrEmpty(_dsDevelopment))
            {
                NameValueCollection systems = ConfigurationManager.GetSection("DDS") as NameValueCollection;
                if (systems != null)
                {
                    _dsDevelopment = systems["Development System"];
                    _dsProduction = systems["Production System"];
                   
                }
            }

            string uriString = $"http://{address}:9678/WCFServiceDDS";
            return new Uri(uriString);
        }

        /// <summary>
        /// messaging system for access to the end points.
        /// </summary>
        public new IContractService Channel
        {
            get { return base.Channel; }
        }
    }
}