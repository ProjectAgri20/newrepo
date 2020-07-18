using System;
using System.Collections.ObjectModel;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Network.Wcf;
using HP.ScalableTest.Print.Monitor;

namespace HP.ScalableTest.Service.Dispatcher
{
    /// <summary>
    /// 
    /// </summary>
    public class DataGatewayService : IDataGatewayService
    {
        /// <summary>
        /// Determines whether job is rendered on client for each queue name.
        /// </summary>
        /// <param name="printServer">The name of the print server being checked</param>
        /// <param name="queues">The list of queues.</param>
        /// <returns>
        ///   <c>true</c> if the job is rendered on the client for each queue name; otherwise, <c>false</c>.
        /// </returns>
        public SerializableDictionary<string, string> GetJobRenderLocation(string printServer, Collection<string> queues)
        {
            if (queues == null)
            {
                throw new ArgumentNullException("queues");
            }

            SerializableDictionary<string, string> jobRenderedData = new SerializableDictionary<string, string>();

            // Create connection to the print monitor client
            using (var client = new WcfClient<IPrintMonitorService>(WcfService.PrintMonitor.GetUri(printServer)))
            {
                TraceFactory.Logger.Debug("Checking {0} queues on {1}".FormatWith(queues.Count, printServer));
                jobRenderedData = client.Channel.GetJobRenderLocation(queues);
            }

            return jobRenderedData;
        }
    }
}
