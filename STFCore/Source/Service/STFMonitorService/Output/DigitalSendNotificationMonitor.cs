using System;
using System.IO;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.DataLog;

namespace HP.ScalableTest.Service.Monitor.Output
{
    /// <summary>
    /// Monitors an Exchange email inbox for Digital Send notification emails.
    /// </summary>
    internal class DigitalSendNotificationMonitor : OutputEmailMonitor
    {
        private const string _subjectStart = "Job Notification: ";

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSendNotificationMonitor"/> class.
        /// </summary>
        /// <param name="monitorConfig">The monitor configuration.</param>
        public DigitalSendNotificationMonitor(MonitorConfig monitorConfig)
            : base(monitorConfig)
        {
        }

        /// <summary>
        /// Processes the message.  Returns true if processing was successful and the message can be deleted.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "The service should not be allowed to crash.")]
        protected override bool ProcessMessage(EmailMessage message)
        {
            // Determine whether this is the kind of message we're looking for
            if (message.Subject.StartsWith(_subjectStart, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    // Extract the file prefix from the body
                    ScanFilePrefix filePrefix = ExtractFileName(message.Body);

                    // Create the log for this email
                    DigitalSendJobNotificationLogger log = new DigitalSendJobNotificationLogger();
                    log.FilePrefix = filePrefix.ToString();
                    log.SessionId = filePrefix.SessionId;
                    log.NotificationResult = message.Subject.Replace(_subjectStart, "");
                    log.NotificationDestination = EmailAddress;
                    log.NotificationReceivedDateTime = message.DateTimeReceived;
                    new DataLogger(GetLogServiceHost(filePrefix.SessionId)).Submit(log);

                    // Success - the service can clean up the email.
                    return true;
                }
                catch
                {
                    // If anything goes wrong, return false so that the service will leave
                    // the email alone.
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static ScanFilePrefix ExtractFileName(string body)
        {
            string faxCode = string.Empty;
            string user = string.Empty;
            string line;
            char[] space = new char[] { ' ' };

            using (StringReader reader = new StringReader(body))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("User:", StringComparison.OrdinalIgnoreCase))
                    {
                        // Grab the user, stripping off the domain if it exists
                        user = line.Split(space, StringSplitOptions.RemoveEmptyEntries)[1];
                        if (user.Contains("\\"))
                        {
                            user = user.Substring(user.IndexOf('\\') + 1);
                        }

                    }
                    else if (line.Contains("-- Destination", StringComparison.OrdinalIgnoreCase))
                    {
                        // The destination info is on the next line.
                        line = reader.ReadLine();
                        faxCode = line.Split(space, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                }
            }

            return ScanFilePrefix.ParseFromFax(faxCode, user);
        }
    }
}
