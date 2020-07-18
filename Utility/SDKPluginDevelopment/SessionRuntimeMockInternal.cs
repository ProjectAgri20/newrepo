using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using HP.ScalableTest.Development;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace SDKPluginDevelopment
{
    /// <summary>
    /// Wrapper around <see cref="SessionRuntimeMock" /> that also implements <see cref="ISessionRuntimeInternal" />.
    /// </summary>
    public sealed class SessionRuntimeMockInternal : ISessionRuntime, ISessionRuntimeInternal
    {
        private readonly SessionRuntimeMock _sessionRuntimeMock;

        /// <summary>
        /// Gets the office worker email addresses that will be returned by <see cref="GetOfficeWorkerEmailAddresses" />.
        /// </summary>
        public Collection<string> OfficeWorkerEmailAddresses { get; } = new Collection<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionRuntimeMockInternal" /> class.
        /// </summary>
        public SessionRuntimeMockInternal()
        {
            _sessionRuntimeMock = new SessionRuntimeMock();
        }

        public MailAddressCollection GetOfficeWorkerEmailAddresses(int count)
        {
            MailAddressCollection addresses = new MailAddressCollection();
            foreach (string address in OfficeWorkerEmailAddresses.Take(count))
            {
                addresses.Add(new MailAddress(address));
            }
            return addresses;
        }

        public void MonitorForDocuments(PluginExecutionData executionData, IDeviceInfo deviceInfo, IEnumerable<string> expectedDocIds)
        {
            // Intentionally left blank
        }

        public bool RequestToSendPrintJob(string server, string queue, int maxJobsInQueue)
        {
            return true;
        }

        public void WaitIfPaused()
        {
            // Intentionally left blank
        }

        public bool IsCitrixEnvironment()
        {
            return false;
        }

        // Reroute all ISessionRuntime calls to the mock
        public void CollectDeviceMemoryProfile(IDeviceInfo deviceInfo, string label) => _sessionRuntimeMock.CollectDeviceMemoryProfile(deviceInfo, label);
        public void ReportAssetError(IAssetInfo assetInfo) => _sessionRuntimeMock.ReportAssetError(assetInfo);
    }
}
