using System.Collections.Generic;
using System.Net.Mail;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Information needed to notify email recipients of consecutive errors in a Session.
    /// </summary>
    public class FailNotificationInfo
    {
        /// <summary>
        /// The SessionId
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// The Session Name
        /// </summary>
        public string SessionName { get; set; }

        /// <summary>
        /// The threshold of Errors
        /// </summary>
        public int RuleValue { get; set; }

        /// <summary>
        /// Whether or not to collect Dart Logs.
        /// </summary>
        public bool CollectDartLog { get; set; }

        /// <summary>
        /// The SmtpServer
        /// </summary>
        public SmtpClient SmtpMail;

        /// <summary>
        /// The email message
        /// </summary>
        public MailMessage Message;

        /// <summary>
        /// The AssetIds of Failure Information
        /// </summary>
        public Dictionary<string, DeviceFailureInfo> FailureInfo { get; }

        /// <summary>
        /// The EmailFrom Address
        /// </summary>
        public string EmailFrom { get; set; }

        /// <summary>
        /// The Smtp Server
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// The SendTo Email Addresses
        /// </summary>
        public string ToAddresses { get; set; }


        /// <summary>
        /// Creates a new instance of FailNotificationInfo
        /// </summary>
        /// <param name="sessionId">The session Id</param>
        /// <param name="sessionName">The session Name</param>
        /// <param name="iterationCutOff">The error threshold</param>
        /// <param name="emailsToSend">The list of email addresse to notify</param>
        /// <param name="collectDartLog">Whether to collect the Dart log.</param>
        public FailNotificationInfo(string sessionId, string sessionName, int iterationCutOff, string emailsToSend, bool collectDartLog)
        {
            try
            {
                EmailFrom = "donotreply@hp.com";
                SmtpServer = GlobalSettings.Items[Setting.AdminEmailServer];

                ToAddresses = emailsToSend;
                SessionId = sessionId;
                RuleValue = iterationCutOff;
                CollectDartLog = collectDartLog;
                SessionName = sessionName;
                FailureInfo = new Dictionary<string, DeviceFailureInfo>();
                SmtpMail = new SmtpClient(SmtpServer);

                Message = new MailMessage { From = new MailAddress(EmailFrom) };
                Message.To.Add(emailsToSend);
                Message.Subject = string.Format("Failed/Error Consecutive Tests for Session: {0}, {1}", SessionId, SessionName);
                Message.Body = string.Format("{0} Consecutive Tests failed for Session: {1}", RuleValue, SessionId);
            }
            catch (System.Exception ex)
            {
                TraceFactory.Logger.Debug(ex);
                TraceFactory.Logger.Debug(ex.Message);
            }
        }
        public void ResetMessageBody()
        {
            Message.Body = string.Format("{0} Consecutive Tests failed for Session: {1}", RuleValue, SessionId);
        }

    }

    /// <summary>
    /// Data class to persist failure information across database queries.
    /// </summary>
    public class DeviceFailureInfo
    {
        /// <summary>
        /// The number of failures for a device
        /// </summary>
        public int FailureCount { get; set; }

        /// <summary>
        /// Whether or not the email has been sent.
        /// </summary>
        public bool EmailSent { get; set; }
    }
}
