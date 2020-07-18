using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using HP.ScalableTest.Development;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Wcf;

namespace PluginSimulator
{
    internal sealed class SessionRuntimeMockInternal : ISessionRuntime, ISessionRuntimeInternal
    {
        private readonly SessionRuntimeMock _sessionRuntimeMock = new SessionRuntimeMock();

        public Collection<string> OfficeWorkerEmailAddresses { get; } = new Collection<string>();

        /// <summary>
        /// Reports a non-functioning asset to the runtime framework.
        /// </summary>
        /// <param name="assetInfo">The <see cref="IAssetInfo" /> representing the asset in error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetInfo" /> is null.</exception>
        public void ReportAssetError(IAssetInfo assetInfo)
        {
            _sessionRuntimeMock.ReportAssetError(assetInfo);
        }

        /// <summary>
        /// Collects a memory usage profile from the specified device.
        /// </summary>
        /// <param name="deviceInfo">The <see cref="IDeviceInfo" /> representing the device to profile.</param>
        /// <param name="label">The label to apply (for logging purposes).</param>
        /// <exception cref="ArgumentNullException"><paramref name="deviceInfo" /> is null.</exception>
        public void CollectDeviceMemoryProfile(IDeviceInfo deviceInfo, string label)
        {
            _sessionRuntimeMock.CollectDeviceMemoryProfile(deviceInfo, label);
        }

        /// <summary>
        /// Retrieves a collection of email addresses for office workers executing in this session.
        /// </summary>
        /// <param name="count">The number of email addresses to retrieve.</param>
        /// <returns>A <see cref="MailAddressCollection" /> containing addresses for this session's office workers.</returns>
        public MailAddressCollection GetOfficeWorkerEmailAddresses(int count)
        {
            MailAddressCollection addresses = new MailAddressCollection();
            foreach (string address in OfficeWorkerEmailAddresses.Take(count))
            {
                addresses.Add(new MailAddress(address));
            }
            return addresses;
        }

        /// <summary>
        /// Monitors the specified device for the provided document IDs and logs when they are found.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="deviceInfo">The <see cref="IDeviceInfo" /> representing the device to monitor.</param>
        /// <param name="expectedDocIds">The document IDs to look for.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executionData" /> is null.
        /// <para>or</para>
        /// <paramref name="deviceInfo" /> is null.
        /// <para>or</para>
        /// <paramref name="expectedDocIds" /> is null.
        /// </exception>
        public void MonitorForDocuments(PluginExecutionData executionData, IDeviceInfo deviceInfo, IEnumerable<string> expectedDocIds)
        {
            if (executionData == null)
            {
                throw new ArgumentNullException(nameof(executionData));
            }

            if (deviceInfo == null)
            {
                throw new ArgumentNullException(nameof(deviceInfo));
            }

            if (expectedDocIds == null)
            {
                throw new ArgumentNullException(nameof(expectedDocIds));
            }

            // Intentionally left blank
        }

        /// <summary>
        /// Determines whether a new print job can be sent to the specified print queue without exceeding the specified number of jobs.
        /// </summary>
        /// <param name="server">The print server.</param>
        /// <param name="queue">The print queue.</param>
        /// <param name="maxJobsInQueue">The maximum number of jobs in the queue.</param>
        /// <returns><c>true</c> if the job can be submitted, <c>false</c> otherwise.</returns>
        /// <exception cref="EndpointNotFoundException">The print monitor service is not running for the specified queue.</exception>
        public bool RequestToSendPrintJob(string server, string queue, int maxJobsInQueue)
        {
            using (PrintMonitorClient printMonitor = new PrintMonitorClient(server))
            {
                return printMonitor.Channel.RequestToSendJob(queue, maxJobsInQueue);
            }
        }

        /// <summary>
        /// Waits for the print job with the specified ID to be finished printing.
        /// </summary>
        /// <param name="server">The print server.</param>
        /// <param name="printJobId">The ID of the print job to wait for.</param>
        /// <param name="timeout">The amount of time to wait for the print job to be finished.</param>
        /// <returns><c>true</c> if the print job finished within the timeout, <c>false</c> otherwise.</returns>
        /// <exception cref="EndpointNotFoundException">The print monitor service is not running for the specified queue.</exception>
        public bool WaitForPrintJob(string server, Guid printJobId, TimeSpan timeout)
        {
            using (PrintMonitorClient printMonitor = new PrintMonitorClient(server))
            {
                return printMonitor.Channel.WaitForPrintJobFinished(printJobId, timeout);
            }
        }

        /// <summary>
        /// Signals a synchronization event with the specified name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public void SignalSynchronizationEvent(string eventName)
        {
            // Intentionally left blank
        }

        /// <summary>
        /// Waits for a synchronization event with the specified name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public void WaitForSynchronizationEvent(string eventName)
        {
            // Intentionally left blank
        }

        /// <summary>
        /// Checks to see whether session execution is paused, and if so, blocks until execution is resumed.
        /// </summary>
        public void WaitIfPaused()
        {
            // Intentionally left blank
        }

        /// <summary>
        /// Determines whether the plugin is running in a Citrix environment.
        /// </summary>
        /// <returns><c>true</c> if the plugin is running in a Citrix environment; otherwise, <c>false</c>.</returns>
        public bool IsCitrixEnvironment()
        {
            return false;
        }

        #region PrintMonitorService helpers

        [ServiceContract]
        private interface IPrintMonitorService
        {
            [OperationContract]
            bool RequestToSendJob(string queueName, int maxJobCount);

            [OperationContract]
            bool WaitForPrintJobFinished(Guid printJobId, TimeSpan timeout);
        }

        private sealed class PrintMonitorClient : WcfClient<IPrintMonitorService>
        {
            public PrintMonitorClient(string address)
                : base(MessageTransferType.Http, $"http://{address}:12666/PrintMonitor")
            {
            }
        }

        #endregion
    }
}
