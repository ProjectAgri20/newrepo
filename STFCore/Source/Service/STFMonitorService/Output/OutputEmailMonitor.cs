using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Email;
using HP.ScalableTest.FileAnalysis;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core.DataLog;

namespace HP.ScalableTest.Service.Monitor.Output
{
    /// <summary>
    /// Monitors an Exchange email inbox for digital send output files.
    /// </summary>
    internal class OutputEmailMonitor : OutputMonitorBase
    {
        private IEmailController _controller;
        private NetworkCredential _credential;
        private MailAddress _emailAddress;
        private string _tempPath;

        private Timer _timer;
        private TimeSpan _timerInterval = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Gets the email address.
        /// </summary>
        protected string EmailAddress
        {
            get { return _emailAddress.ToString(); }
        }

        /// <summary>
        /// Gets the location to monitor.
        /// </summary>
        public override string MonitorLocation
        {
            get { return EmailAddress; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputEmailMonitor"/> class.
        /// </summary>
        /// <param name="monitorConfig">The monitor configuration.</param>
        public OutputEmailMonitor(MonitorConfig monitorConfig) 
            : base(monitorConfig)
        {
            _timer = new Timer(new TimerCallback(TimerFire));

            // Set up the email controller for this domain
            string password = GlobalSettings.Items[Setting.OfficeWorkerPassword];
            string domain = GlobalSettings.Items[Setting.Domain];
            _credential = new NetworkCredential(base.Configuration.MonitorLocation, password, domain);
            _emailAddress = ExchangeEmailController.GetLdapEmailAddress(_credential);

            // Set up a temporary path and make sure it exists
            _tempPath = Path.Combine(Path.GetTempPath(), "EmailMonitor", base.Configuration.MonitorLocation);
            System.IO.Directory.CreateDirectory(_tempPath);
        }

        private void ConfigureEmailController()
        {
            SettingsDictionary exchangeServerSettings = null;

            using (AssetInventoryContext aiContext = DbConnect.AssetInventoryContext())
            {
                string serverType = ServerType.Exchange.ToString();
                FrameworkServer server = aiContext.FrameworkServers.FirstOrDefault(n => n.ServerTypes.Any(m => m.Name == serverType) && n.Active);
                exchangeServerSettings = new SettingsDictionary(server.ServerSettings.ToDictionary(n => n.Name, n => n.Value));
            }

            ExchangeConnectionSettings settings = new ExchangeConnectionSettings(exchangeServerSettings);
            if (settings.AutodiscoverEnabled)
            {
                TraceFactory.Logger.Debug("Configuring exchange service using Autodiscover.");
                _controller = new ExchangeEmailController(_credential, _emailAddress);
            }
            else
            {
                TraceFactory.Logger.Debug("Configuring exchange service using FrameworkServerSetting URL.");
                _controller = new ExchangeEmailController(_credential, settings.EwsUrl);
            }
        }

        private void TimerFire(object notUsed)
        {
            _timer.Change(-1, -1);
            ProcessExisting();
            _timer.Change(_timerInterval, _timerInterval);
        }

        /// <summary>
        /// Processes existing files in the destination being monitored.
        /// </summary>
        private void ProcessExisting()
        {
            Console.WriteLine("Processing existing emails for '{0}'.".FormatWith(_emailAddress));
            Collection<EmailMessage> allmessages = null;
            Collection<EmailMessage> processedMessages = new Collection<EmailMessage>();

            try
            {
                allmessages = _controller.RetrieveMessages(EmailFolder.Inbox);
            }
            catch (EmailServerException ex)
            {
                //Something's up with the Email server.
                TraceFactory.Logger.Error(ex);
                return;
            }
            
            // Process each email, and note which ones are successful
            foreach (EmailMessage message in allmessages)
            {
                bool success = ProcessMessage(message);
                if (success)
                {
                    processedMessages.Add(message);
                }
            }

            // Remove all the successful messages from the server
            _controller.Delete(processedMessages);
        }

        /// <summary>
        /// Processes the message.  Returns true if processing was successful and the message can be deleted.
        /// </summary>
        /// <param name="message">The message.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "The service should not be allowed to crash.")]
        protected virtual bool ProcessMessage(EmailMessage message)
        {
            EmailAttachment attachment = message.Attachments.FirstOrDefault();
            if (attachment != null)
            {
                // Determine if we need to pull the file name from the subject
                string fileName = attachment.FileName;
                if (!ScanFilePrefix.MatchesPattern(attachment.FileName))
                {
                    fileName = message.Subject + Path.GetExtension(attachment.FileName);
                }
                TraceFactory.Logger.Debug("Found attachment: " + fileName);

                try
                {
                    ScanFilePrefix filePrefix = ScanFilePrefix.Parse(Path.GetFileName(fileName));

                    // Create the log for this file
                    DigitalSendJobOutputLogger log = new DigitalSendJobOutputLogger(fileName, filePrefix.ToString(), filePrefix.SessionId);
                    log.FileSentDateTime = message.DateTimeSent;
                    log.FileReceivedDateTime = message.DateTimeReceived;
                    log.FileLocation = _emailAddress.ToString();

                    // Save the attachment locally
                    FileInfo file = attachment.Save(_tempPath, fileName);

                    // Validate and analyze the file
                    OutputProcessor processor = new OutputProcessor(file.FullName);
                    ValidationResult result = null;
                    Retry.WhileThrowing(
                        () => result = processor.Validate(base.Configuration),
                        10,
                        TimeSpan.FromSeconds(2),
                        new Collection<Type>() { typeof(IOException) });

                    DocumentProperties properties = processor.GetProperties();
                    log.FileSizeBytes = properties.FileSize;
                    log.PageCount = properties.Pages;
                    log.SetErrorMessage(result);

                    // Clean up the file
                    processor.ApplyRetention(base.Configuration, result.Succeeded);

                    // One last check - if there was more than one attachment, flag this as an error
                    if (message.Attachments.Count > 1)
                    {
                        log.ErrorMessage += " {0} attachments with one email.".FormatWith(message.Attachments.Count);
                    }

                    // Send the output log
                    new DataLogger(GetLogServiceHost(filePrefix.SessionId)).Submit(log);
                }
                catch (Exception ex)
                {
                    LogProcessFileError(fileName, ex);
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Debug("Found email with subject {0} but no attachments.".FormatWith(message.Subject));
            }

            return true;
        }

        private static void LogProcessFileError(string fileName, Exception ex)
        {
            TraceFactory.Logger.Error("{0} could not be processed.".FormatWith(fileName), ex);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void StartMonitoring()
        {
            if (_controller == null)
            {
                ConfigureEmailController();
            }

            ProcessExisting();

            _timer.Change(_timerInterval, _timerInterval);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void StopMonitoring()
        {
            _timer.Change(-1, -1);
        }

        public override string ToString() => $"Email Monitor for {EmailAddress}";

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        #endregion
    }
}
