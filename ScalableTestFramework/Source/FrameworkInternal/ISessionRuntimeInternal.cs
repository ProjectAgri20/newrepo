using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.ServiceModel;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Internal extensions to <see cref="ISessionRuntime" />.
    /// </summary>
    public interface ISessionRuntimeInternal : ISessionRuntime
    {
        /// <summary>
        /// Retrieves a collection of email addresses for office workers executing in this session.
        /// </summary>
        /// <param name="count">The number of email addresses to retrieve.</param>
        /// <returns>A <see cref="MailAddressCollection" /> containing addresses for this session's office workers.</returns>
        MailAddressCollection GetOfficeWorkerEmailAddresses(int count);

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
        void MonitorForDocuments(PluginExecutionData executionData, IDeviceInfo deviceInfo, IEnumerable<string> expectedDocIds);

        /// <summary>
        /// Determines whether a new print job can be sent to the specified print queue without exceeding the specified number of jobs.
        /// </summary>
        /// <param name="server">The print server.</param>
        /// <param name="queue">The print queue.</param>
        /// <param name="maxJobsInQueue">The maximum number of jobs in the queue.</param>
        /// <returns><c>true</c> if the job can be submitted, <c>false</c> otherwise.</returns>
        /// <exception cref="EndpointNotFoundException">The print monitor service is not running for the specified queue.</exception>
        bool RequestToSendPrintJob(string server, string queue, int maxJobsInQueue);

        /// <summary>
        /// Waits for the print job with the specified ID to be finished printing.
        /// </summary>
        /// <param name="server">The print server.</param>
        /// <param name="printJobId">The ID of the print job to wait for.</param>
        /// <param name="timeout">The amount of time to wait for the print job to be finished.</param>
        /// <returns><c>true</c> if the print job finished within the timeout, <c>false</c> otherwise.</returns>
        /// <exception cref="EndpointNotFoundException">The print monitor service is not running for the specified server.</exception>
        bool WaitForPrintJob(string server, Guid printJobId, TimeSpan timeout);

        /// <summary>
        /// Signals a synchronization event with the specified name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        void SignalSynchronizationEvent(string eventName);

        /// <summary>
        /// Waits for a synchronization event with the specified name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        void WaitForSynchronizationEvent(string eventName);

        /// <summary>
        /// Checks to see whether session execution is paused, and if so, blocks until execution is resumed.
        /// </summary>
        void WaitIfPaused();

        /// <summary>
        /// Determines whether the plugin is running in a Citrix environment.
        /// </summary>
        /// <returns><c>true</c> if the plugin is running in a Citrix environment; otherwise, <c>false</c>.</returns>
        bool IsCitrixEnvironment();
    }
}
