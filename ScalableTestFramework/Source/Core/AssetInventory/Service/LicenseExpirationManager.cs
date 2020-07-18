using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.AssetInventory.Service
{
    /// <summary>
    /// Manages software licenses that are expiring.
    /// </summary>
    public sealed class LicenseExpirationManager : IAssetInventoryMaintenanceManager
    {
        private readonly AssetInventoryConnectionString _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseExpirationManager" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="AssetInventoryConnectionString" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public LicenseExpirationManager(AssetInventoryConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Sends notifications for all expiring licenses.
        /// </summary>
        /// <param name="expirationNotifier">The <see cref="ExpirationNotifier" />.</param>
        public void SendExpirationNotifications(ExpirationNotifier expirationNotifier)
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                List<License> expiringLicenses = context.Licenses.Include(n => n.Owners)
                    .Where(n => n.RequestSentDate == null && DbFunctions.AddDays(DateTime.Now, n.ExpirationNoticeDays) > n.ExpirationDate)
                    .ToList();
                LogDebug($"Found {expiringLicenses.Count} expiring licenses.");

                IEnumerable<Guid> serverIds = expiringLicenses.Select(n => n.FrameworkServerId).Distinct();
                Dictionary<Guid, FrameworkServer> servers = context.FrameworkServers
                    .Where(n => serverIds.Contains(n.FrameworkServerId))
                    .ToDictionary(n => n.FrameworkServerId, n => n);

                foreach (License license in expiringLicenses)
                {
                    servers.TryGetValue(license.FrameworkServerId, out FrameworkServer server);

                    bool sentRenewalRequest = false;
                    if (license.SolutionVersion.Equals("latest", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //If not set to "latest" the licence renewal will be done manually
                        SendNewLicenseRequest(license, server);
                        sentRenewalRequest = true;
                    }

                    SendExpirationNotification(expirationNotifier, license, server, sentRenewalRequest);

                    license.RequestSentDate = DateTime.Now;
                    context.SaveChanges();
                }

                LogDebug("Finished processing license notifications.");
            }
        }

        private static void SendExpirationNotification(ExpirationNotifier notifier, License license, FrameworkServer server, bool sentRenewalRequest)
        {
            if (license.Owners.Count == 0)
            {
                LogError($"Unable to notify license expiration for '{server.HostName}'.  No contacts specified.");
                return;
            }

            LicenseOwner owner = license.Owners.FirstOrDefault();
            try
            {
                MailAddress recipient = new MailAddress(owner.Contact);
                string subject = string.Format(Resource.LicenseEmailSubject, license.Solution, server.HostName);
                string body = sentRenewalRequest ?
                    string.Format(Resource.LicenseEmailBodyAuto, license.Solution, server.HostName, license.ExpirationDate.ToShortDateString(), Environment.MachineName) : 
                    string.Format(Resource.LicenseEmailBodyManual, license.Solution, server.HostName, license.ExpirationDate.ToShortDateString(), Environment.MachineName);
                notifier.SendNotification(recipient, subject, body);
            }
            catch (FormatException ex)
            {
                // Notification recipient is not a valid email address
                LogDebug(ex.ToString());
            }

        }

        private static void SendNewLicenseRequest(License license, FrameworkServer server)
        {
            if (license.Owners.Count == 0)
            {
                LogError($"Unable to request new license for '{server.HostName}'.  No contacts specified.");
                return;
            }

            LicenseOwner owner = license.Owners.FirstOrDefault();
            string formattedOwner = owner.Contact.Split('@')[0];
            string requestData = string.Format(Resource.LicenseRequestData, formattedOwner, BuildRequestDetails(license, server));

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Resource.LicenseTicketURL + "?" + requestData);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Proxy = null;
            webRequest.UseDefaultCredentials = true; // Uses Windows Credentials to verify user
            HttpWebResult webResult = HttpWebEngine.Get(webRequest);
            LogInfo($"Response from ACT Solution Support Ticket System: {webResult.Response}");
        }

        private static string BuildRequestDetails(License license, FrameworkServer server)
        {
            StringBuilder result = new StringBuilder("Solution: ");

            result.AppendLine(license.Solution);
            result.Append("Hostname: ").AppendLine(server.HostName);
            if (!string.IsNullOrEmpty(license.InstallationKey))
            {
                result.Append("ID Key: ").AppendLine(license.InstallationKey);
            }
            result.Append("Version: ").AppendLine(license.SolutionVersion);
            result.Append("OS: ").AppendLine(server.OperatingSystem);
            result.Append("Seats Requested: ").AppendLine(license.Seats.ToString());

            return result.ToString();
        }

        #region Explicit IAssetInventoryMaintenanceManager Members

        void IAssetInventoryMaintenanceManager.RunMaintenance()
        {
            // Nothing else to do in this class
        }

        #endregion
    }
}
