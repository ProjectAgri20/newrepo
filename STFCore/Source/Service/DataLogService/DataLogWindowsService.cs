using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory.Reservation;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DataLog.Service;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Service.DataLog
{
    /// <summary>
    /// Windows service that hosts the Central Log service.
    /// </summary>
    public class DataLogWindowsService : SelfInstallingServiceBase
    {
        private static readonly int _retryNotificationThreshold = 5;
        private static readonly TimeSpan _cacheCheckFrequency = TimeSpan.FromMinutes(1);

        private ServiceHost _dataLogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogWindowsService"/> class.
        /// </summary>
        public DataLogWindowsService()
            : base("DataLogService", "STF Data Log Service")
        {
            Description = "STF service to collect and save data from executing components.";
        }

        /// <summary>
        /// Starts this service instance.
        /// </summary>
        /// <param name="args">The <see cref="CommandLineArguments" /> provided to the start command.</param>
        protected override void StartService(CommandLineArguments args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            TraceFactory.Logger.Debug("Starting Data Log service...");

            // Load the settings, either from the command line or the local cache
            SettingsLoader.LoadSystemConfiguration(args.GetParameterValue("database"));

            // Create the data log service implementation
            DataLogService singletonInstance;
            string alternateDatabase = SettingsLoader.RetrieveSetting("DataLogDatabaseAlternate");
            if (!string.IsNullOrEmpty(alternateDatabase))
            {
                singletonInstance = new DataLogService(DbConnect.DataLogConnectionString, new DataLogConnectionString(alternateDatabase));
            }
            else
            {
                singletonInstance = new DataLogService(DbConnect.DataLogConnectionString);
            }

            // Set data log configuration
            singletonInstance.InitializeCache(new DirectoryInfo("Cache"), _cacheCheckFrequency);
            singletonInstance.CacheOperationsRetried += OnCacheOperationsRetried;
            singletonInstance.SessionDataExpired += OnSessionDataExpired;

            // Start the service
            _dataLogService = new WcfHost<IDataLogService>(singletonInstance, DataLogServiceEndpoint.MessageTransferType, DataLogServiceEndpoint.BuildUri("localhost"));
            _dataLogService.Open();

            TraceFactory.Logger.Debug("Data Log service started.");
        }

        /// <summary>
        /// Stops this service instance.
        /// </summary>
        protected override void StopService()
        {
            _dataLogService.Close();
        }

        private void OnSessionDataExpired(object sender, DataLogCleanupEventArgs e)
        {
            if (e.SessionIds.Any())
            {
                AssetReservationManager assetManager = new AssetReservationManager(DbConnect.AssetInventoryConnectionString, null);
                FrameworkClientReservationManager clientManager = new FrameworkClientReservationManager(DbConnect.AssetInventoryConnectionString);
                DomainAccountReservationManager accountManager = new DomainAccountReservationManager(DbConnect.AssetInventoryConnectionString);

                assetManager.ReleaseSessionReservations(e.SessionIds);
                clientManager.ReleaseSessionReservations(e.SessionIds);
                accountManager.ReleaseSessionReservations(e.SessionIds);
            }
        }

        private void OnCacheOperationsRetried(object sender, DataLogCacheEventArgs e)
        {
            var notifyResults = e.Results.Where(n => !n.Success && (n.Retries == _retryNotificationThreshold || n.Retries < 0));
            foreach (var result in notifyResults.GroupBy(n => n.Table))
            {
                TraceFactory.Logger.Debug($"Sending failure notification for {result.Key} table.");
                SendFailureMessage(result.Key, result);
            }
        }

        private void SendFailureMessage(string tableName, IEnumerable<DataLogDatabaseResult> results)
        {
            // Some email clients (e.g. Outlook) will sometimes strip out the line breaks in the parameter list,
            // causing them all to appear on one line.  This is a little hack to work around that.
            string parametersNewLine = "\t\r\n";

            StringBuilder msgBody = new StringBuilder("The following error(s) occurred when logging test data:").AppendLine();
            foreach (DataLogDatabaseResult result in results)
            {
                msgBody.AppendLine("==========");
                msgBody.AppendLine();
                msgBody.AppendLine("SQL COMMAND STATEMENT:");
                msgBody.AppendLine(result.Command);
                msgBody.AppendLine();
                msgBody.AppendLine("SQL PARAMETERS:");
                msgBody.AppendLine(string.Join(parametersNewLine, result.Parameters.Select(n => $"{n.Key} = {n.Value}")));
                msgBody.AppendLine();
                msgBody.AppendLine("ERROR MESSAGE:");
                msgBody.AppendLine(result.Error);
                msgBody.AppendLine();
            }

            List<string> emailToList = SettingsLoader.RetrieveSetting(Setting.AdminEmailAddress.ToString()).Split(';').ToList();
            using (SmtpClient client = new SmtpClient(SettingsLoader.RetrieveSetting(Setting.AdminEmailServer.ToString())))
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress("donotreply@hp.com", "STF Logging Error");
                    message.To.Add(string.Join(",", emailToList));
                    message.ReplyToList.Add(string.Join(",", emailToList));
                    message.Subject = $"STF Logging Error Report in table {tableName} on server {Environment.MachineName}";
                    message.Body = msgBody.ToString();

                    client.Send(message);
                }
            }
        }
    }
}
