using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using HP.ScalableTest.Core;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor;

namespace HP.ScalableTest.Framework.PluginService
{
    /// <summary>
    /// Provides access to runtime components available when executing a test session.
    /// </summary>
    public sealed class SessionRuntimeAdapter : ISessionRuntime, ISessionRuntimeInternal
    {
        /// <summary>
        /// Reports a non-functioning asset to the runtime framework.
        /// </summary>
        /// <param name="assetInfo">The <see cref="IAssetInfo" /> representing the asset in error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetInfo" /> is null.</exception>
        public void ReportAssetError(IAssetInfo assetInfo)
        {
            if (assetInfo == null)
            {
                throw new ArgumentNullException(nameof(assetInfo));
            }

            RuntimeError assetError = new RuntimeError(assetInfo.AssetId);
            SessionProxyBackendConnection.HandleAssetError(assetError);
        }

        /// <summary>
        /// Collects a memory usage profile from the specified device.
        /// </summary>
        /// <param name="deviceInfo">The <see cref="IDeviceInfo" /> representing the device to profile.</param>
        /// <param name="label">The label to apply (for logging purposes).</param>
        /// <exception cref="ArgumentNullException"><paramref name="deviceInfo" /> is null.</exception>
        public void CollectDeviceMemoryProfile(IDeviceInfo deviceInfo, string label)
        {
            if (deviceInfo == null)
            {
                throw new ArgumentNullException(nameof(deviceInfo));
            }

            //Manually serialize the IDeviceInfo object before going over the wire.
            string serializedDeviceInfo = Serializer.Serialize(deviceInfo).ToString();

            SessionProxyBackendConnection.CollectDeviceMemoryProfile(serializedDeviceInfo, label);
        }

        /// <summary>
        /// Retrieves a collection of email addresses for office workers executing in this session.
        /// </summary>
        /// <param name="count">The number of email addresses to retrieve.</param>
        /// <returns>A <see cref="MailAddressCollection" /> containing addresses for this session's office workers.</returns>
        public MailAddressCollection GetOfficeWorkerEmailAddresses(int count)
        {
            MailAddressCollection addresses = new MailAddressCollection();

            // Determine which office workers are running in this session
            var officeWorker = GlobalDataStore.Manifest.Resources.OfType<OfficeWorkerDetail>().FirstOrDefault();
            if (officeWorker == null)
            {
                return addresses;
            }

            // Create a shuffled list of all the potential recipients
            List<EmailBuilder> userEmails = new List<EmailBuilder>();
            foreach (int i in Enumerable.Range(officeWorker.StartIndex, officeWorker.OfficeWorkerCount))
            {
                string user = string.Format(officeWorker.UserNameFormat, i);
                string domain = GlobalSettings.Items[Setting.DnsDomain];
                userEmails.Add(new EmailBuilder(user, domain));
            }
            userEmails.Shuffle();

            // Select the requested number from the shuffled list
            foreach (EmailBuilder selected in userEmails.Take(count))
            {
                addresses.Add(selected.Address);
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

            try
            {
                // Send expected document ids to monitoring service offline activity execution critical path
                var client = PhysicalDeviceJobLogMonitorServiceClient.Create();
                if (client == null)
                {
                    ExecutionServices.SystemTrace.LogInfo("Physical device monitor is not enabled/defined, skipping");
                }
                else
                {
                    IPhysicalDeviceJobLogMonitorService proxyClient = client.Channel;

                    proxyClient.MonitorDeviceForDocumentIds
                    (
                        executionData.SessionId,
                        Guid.Empty,  // formerly the activity id, not really being used now
                        executionData.ActivityExecutionId,
                        deviceInfo.AssetId,
                        deviceInfo.Address,
                        expectedDocIds.ToList(),
                        "admin",
                        deviceInfo.AdminPassword
                    );
                }
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogError("Error communicating with PhysicalDeviceJobLogMonitorServiceClient", ex);
            }
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
            using (var client = new WcfClient<IPrintMonitorService>(MessageTransferType.Http, WcfService.PrintMonitor.GetHttpUri(server)))
            {
                return client.Channel.RequestToSendJob(queue, maxJobsInQueue);
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
            using (var client = new WcfClient<IPrintMonitorService>(MessageTransferType.Http, WcfService.PrintMonitor.GetHttpUri(server)))
            {
                return client.Channel.WaitForPrintJobFinished(printJobId, timeout);
            }
        }

        /// <summary>
        /// Signals a synchronization event with the specified name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public void SignalSynchronizationEvent(string eventName)
        {
            SessionProxyBackendConnection.SignalSynchronizationEvent(eventName);
        }

        /// <summary>
        /// Waits for a synchronization event with the specified name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public void WaitForSynchronizationEvent(string eventName)
        {
            ApplicationFlowControl.Instance.WaitForSync(eventName);
        }

        /// <summary>
        /// Checks to see whether session execution is paused, and if so, blocks until execution is resumed.
        /// </summary>
        public void WaitIfPaused()
        {
            Action logPause = () => Logger.LogInfo("Plugin executed is paused.");
            Action logResume = () => Logger.LogInfo("Plugin execution has resumed.");
            ApplicationFlowControl.Instance.CheckWait(logPause, logResume);
        }

        /// <summary>
        /// Determines whether the plugin is running in a Citrix environment.
        /// </summary>
        /// <returns><c>true</c> if the plugin is running in a Citrix environment; otherwise, <c>false</c>.</returns>
        public bool IsCitrixEnvironment()
        {
            return GlobalDataStore.Manifest.ResourceType == VirtualResourceType.CitrixWorker;
        }
    }
}
