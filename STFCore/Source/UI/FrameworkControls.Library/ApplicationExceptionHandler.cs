using System;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.UI.Framework
{
    /// <summary>
    /// Class for handling unhandled exceptions in an interactive application.
    /// </summary>
    public static class ApplicationExceptionHandler
    {
        private const string SmtpHost = "smtp3.hp.com";
        private const string EmailAddress = "stf.developers@hp.com";
        private const string DoNotReply = "donotreply@hp.com";

        private static Form _mainForm;
        private static DateTime _timeStamp;

        /// <summary>
        /// Attaches this instance to the specified application.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        public static void Attach(Form mainForm)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            _mainForm = mainForm;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            UnhandledException(e.ExceptionObject as Exception);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            UnhandledException(e.Exception);
        }

        /// <summary>
        /// Handles an unhandled exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void UnhandledException(Exception ex)
        {
            _timeStamp = DateTime.Now;

            using (ExceptionDialog dialog = new ExceptionDialog(ex))
            {
                dialog.SubmitError += new EventHandler<ExceptionDetailEventArgs>(messageBox_SubmitError);
                dialog.UserName = UserManager.CurrentUserName.ToUpperInvariant();

                DialogResult result = dialog.ShowDialog(_mainForm);
                if (result == DialogResult.Abort)
                {
                    Application.Exit();
                }
            }
        }

        /// <summary>
        /// Handles an unhandled exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="detail">The detail.</param>
        /// <param name="userName">Name of the user.</param>
        public static void UnhandledException(string message, string detail, string userName = null)
        {
            _timeStamp = DateTime.Now;

            using (ExceptionDialog dialog = new ExceptionDialog(message, detail))
            {
                dialog.SubmitError += new EventHandler<ExceptionDetailEventArgs>(messageBox_SubmitError);
                if (!string.IsNullOrEmpty(userName))
                {
                    dialog.UserName = userName;
                }

                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.Abort)
                {
                    Application.Exit();
                }
            }
        }

        private static void messageBox_SubmitError(object sender, ExceptionDetailEventArgs e)
        {
            ExceptionDialog dialog = sender as ExceptionDialog;

            if (string.IsNullOrWhiteSpace(dialog.UserNotes))
            {
                MessageBox.Show("Please enter some details to help us debug this issue.",
                                "Details Required",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            MailAddress ccAddress = null;
            if (!string.IsNullOrEmpty(dialog.UserEmail))
            {
                try
                {
                    ccAddress = new MailAddress(dialog.UserEmail);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message,
                                    "Invalid Email",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
            }

            SendEmail(ConstructMessage(dialog), ccAddress);
            dialog.SubmitSuccessful = true;
        }

        private static void SendEmail(string body, MailAddress ccEmail = null)
        {
            using (SmtpClient client = new SmtpClient(SmtpHost))
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(DoNotReply, "STF Error Reporting");
                    message.To.Add(new MailAddress(EmailAddress));
                    message.ReplyToList.Add(EmailAddress);
                    if (ccEmail != null)
                    {
                        message.CC.Add(ccEmail);
                        message.ReplyToList.Add(ccEmail);
                    }

                    message.Subject = "STF Error Report " + DateTime.Now.Ticks.ToString("X16", CultureInfo.InvariantCulture);
                    message.Body = body;
                    try
                    {
                        client.Send(message);
                    }
                    catch
                    {
                        // Since a call to GlobalSettings may not be available at this point,
                        // just ignore the exception if we cannot send an email using the specified SMTP server.
                    }
                }
            }
        }

        private static string ConstructMessage(ExceptionDialog messageBox)
        {
            StringBuilder message = new StringBuilder();

            // Add the exception message and user details
            message.AppendFormat("The following error occurred in {0} at {1}:\n", messageBox.AssemblyName, _timeStamp);
            message.AppendLine(messageBox.ExceptionMessage);
            message.AppendLine();
            message.AppendLine("USER INFORMATION:");
            message.AppendLine(messageBox.UserNotes);
            message.AppendLine();

            // Add the environment details
            message.AppendLine("ENVIRONMENT DETAILS:");
            message.AppendLine("User Name: " + messageBox.UserName);
            message.AppendLine("Host Name: " + messageBox.HostName);
            message.AppendLine("Host User: " + messageBox.HostUser);
            message.AppendLine("Address: " + messageBox.Address);
            message.AppendLine("Assembly: " + messageBox.AssemblyName);
            message.AppendLine("Version: " + messageBox.AssemblyVersion);
            message.AppendLine();

            // Add the exception detail
            message.AppendLine("EXCEPTION DETAILS:");
            message.AppendLine(messageBox.ExceptionDetail);

            return message.ToString();
        }
    }
}
