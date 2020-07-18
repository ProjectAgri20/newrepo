using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Email
{
    [ToolboxItem(false)]
    public partial class EmailExecutionControl : UserControl, IPluginExecutionEngine
    {
        private IEmailController _emailController = null;
        private bool _controllerConfigured = false;

        /// Contains a copy of the current Send Email activity being worked on.
        private EmailActivityData _sendActivity = new EmailActivityData();

        /// Control for managing the Email Inbox
        private ExchangeInboxControl _exchangeInboxControl;

        /// <summary>
        /// Constructs the Exchange Email control.
        /// </summary>
        public EmailExecutionControl()
        {
            InitializeComponent();
        }

        private void LoadInbox()
        {
            ExecutionServices.SystemTrace.LogDebug("Loading inbox control");

            // Set up the control that displays the Email messages.
            _exchangeInboxControl = new ExchangeInboxControl(_emailController);
            SuspendLayout();
            _exchangeInboxControl.Location = new Point(57, -83);
            _exchangeInboxControl.Dock = DockStyle.Fill;
            _exchangeInboxControl.Name = "_mapiInboxControl";
            _exchangeInboxControl.Size = new Size(150, 150);
            _exchangeInboxControl.TabIndex = 0;
            emailReceivePanel.Controls.Add(_exchangeInboxControl);
            ResumeLayout(false);
        }

        /// <summary>
        /// Populate the email form.
        /// </summary>
        /// <param name="email">Email details to populate the form with.</param>
        private void PopulateControl(EmailActivityData email, DocumentCollection attachments, IEnumerable<MailAddress> toRecipients, IEnumerable<MailAddress> ccRecipients)
        {
            ExecutionServices.SystemTrace.LogDebug("Clearing form");
            ClearForm();

            ExecutionServices.SystemTrace.LogDebug("Populating fields");
            TypeText(emailToTextBox, string.Join(";", toRecipients), 10);
            TypeText(emailCcTextBox, string.Join(";", ccRecipients), 10);
            TypeText(emailSubjectTextBox, email.Subject, 75);
            TypeText(emailBodyTextBox, email.Body, 75);

            ExecutionServices.SystemTrace.LogDebug("Adding attachments");
            AddAttachmentsToList(attachments);
        }

        /// <summary>
        /// Adds a list of attachments to the ListBox
        /// </summary>
        /// <param name="attachments">Attachments to add.</param>
        private void AddAttachmentsToList(DocumentCollection attachments)
        {
            foreach (var attachment in attachments)
            {
                string attachmentLocation = ExecutionServices.FileRepository.GetFile(attachment).FullName;
                AddAttachmentToList(attachmentLocation);
                Thread.Sleep(75);
            }
        }

        /// <summary>
        /// Adds an attachement to the ListBox.
        /// This process is done on the UI thread.
        /// </summary>
        /// <param name="attachment">Attachment to add.</param>
        private void AddAttachmentToList(string attachment)
        {
            if (emailAttachmentsListBox.InvokeRequired)
            {
                emailAttachmentsListBox.Invoke(new MethodInvoker(() => AddAttachmentToList(attachment)));
                return;
            }

            emailAttachmentsListBox.Items.Add(attachment);
        }

        /// <summary>
        /// Clear all of the controls in the email form.
        /// </summary>
        private void ClearForm()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(ClearForm));
                return;
            }

            emailToTextBox.Text = string.Empty;
            emailCcTextBox.Text = string.Empty;
            emailSubjectTextBox.Text = string.Empty;
            emailBodyTextBox.Text = string.Empty;
            emailAttachmentsListBox.Items.Clear();
        }

        /// <summary>
        /// Eye candy for typing text into a TextBox field.
        /// </summary>
        /// <param name="field">Field to type into</param>
        /// <param name="data">Text to type</param>
        /// <param name="delay">How long to delay between each character being typed.</param>
        private void TypeText(TextBox field, string data, int delay)
        {
            if (field.InvokeRequired)
            {
                field.Invoke(new MethodInvoker(() => TypeText(field, data, delay)));
                return;
            }

            field.Clear();
            foreach (char c in data.ToCharArray())
            {
                field.AppendText(c.ToString());
                Application.DoEvents();
                Thread.Sleep(delay);
            }
            ExecutionServices.SystemTrace.LogNotice($"textbox={field.Name}, text={data}");
        }

        private void SendEmail()
        {
            using (MailMessage message = new MailMessage())
            {
                foreach (string toField in emailToTextBox.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(toField.Trim());
                }
                foreach (string ccField in emailCcTextBox.Text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.CC.Add(ccField.Trim());
                }

                message.Subject = emailSubjectTextBox.Text;
                message.BodyEncoding = System.Text.Encoding.ASCII;
                message.Body = emailBodyTextBox.Text;

                //Add Tracker info to the email header
                EmailTracker.Tag(message);

                List<FileInfo> attachments = new List<FileInfo>();
                foreach (string attachment in emailAttachmentsListBox.Items)
                {
                    attachments.Add(new FileInfo(attachment));
                }

                // Send the Email.
                _emailController.Send(message, attachments);
                ExecutionServices.SystemTrace.LogInfo("Email message sent");
            }
        }

        /// <summary>
        /// User clicked the "Send" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void emailSendButton_Click(object sender, EventArgs e)
        {
            // Eye candy that the button was clicked.
            emailSendButton.BackColor = Color.LightGreen;
            emailSendButton.Enabled = false;
            emailSendButton.Refresh();

            // Send the e-mail in the background, so we don't lock up the UI.
            ThreadPool.QueueUserWorkItem(sendEmailWorker_DoWork);
        }

        private void sendEmailWorker_DoWork(object notUsed)
        {
            Thread.CurrentThread.SetName("SendEmailWorker");
            SendEmail();
            UndoClickVisuals();
        }

        private void UndoClickVisuals()
        {
            if (emailSendButton.InvokeRequired)
            {
                emailSendButton.Invoke(new MethodInvoker(UndoClickVisuals));
                return;
            }

            // Eye candy that the "click" event is done.
            emailSendButton.BackColor = SystemColors.Control;
            emailSendButton.Enabled = true;
            emailSendButton.Refresh();
        }

        /// <summary>
        /// User clicked the "Refresh" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshButton_Click(object sender, EventArgs e)
        {
            _exchangeInboxControl.LoadInbox();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            try
            {
                ServerInfo exchangeServerInfo = executionData.Servers.First();

                if (!_controllerConfigured)
                {
                    // Alan Andrews 7/9/2010:
                    // For some reason, Autodiscover hangs when we do not have this VMLevelLock in place.
                    // Putting the lock in, then Autodiscover no longer hangs.  I don't quite understand why, but
                    // this is how automation goes sometimes.

                    var action = new Action(() =>
                    {
                        ExecutionServices.SystemTrace.LogDebug("Autodiscover lock acquired");
                        ExchangeConnectionSettings settings = new ExchangeConnectionSettings(exchangeServerInfo);
                        _emailController = new ExchangeEmailController(executionData.Credential, settings);
                    });

                    ExecutionServices.CriticalSection.Run(new ExchangeAutodiscoverLockToken(new TimeSpan(0, 5, 0), new TimeSpan(0, 5, 0)), action);

                    if (InvokeRequired)
                    {
                        ExecutionServices.SystemTrace.LogDebug("Invoking Load Inbox activity");
                        Invoke(new Action(LoadInbox));
                    }

                    _controllerConfigured = true;

                }

                LogUsageData(executionData, exchangeServerInfo);

                _sendActivity = executionData.GetMetadata<EmailActivityData>();
                ExecutionServices.SystemTrace.LogDebug("Activity deserialized, Preparing to send an email");

                var allRecipients = ExecutionServices.SessionRuntime.AsInternal().GetOfficeWorkerEmailAddresses(_sendActivity.ToRandomCount + _sendActivity.CCRandomCount);
                ExecutionServices.SystemTrace.LogDebug("Adding To: recipients");
                var toRecipients = allRecipients.Take(_sendActivity.ToRandomCount);
                ExecutionServices.SystemTrace.LogDebug("Adding CC: recipients");
                var ccRecipients = allRecipients.Skip(_sendActivity.ToRandomCount).Take(_sendActivity.CCRandomCount);

                PopulateControl(_sendActivity, executionData.Documents, toRecipients, ccRecipients);
                SendEmail();

                ExecutionServices.SystemTrace.LogDebug("Email sent, now sleeping 1 second");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            finally
            {
                // We want to attempt an email receive, even if something goes wrong with the send
                if (_exchangeInboxControl != null)
                {
                    _exchangeInboxControl.LoadInbox();
                }
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        private void LogUsageData(PluginExecutionData executionData, ServerInfo server)
        {
            //Log Server usage
            ActivityExecutionServerUsageLog serverLog = new ActivityExecutionServerUsageLog(executionData, server);
            ExecutionServices.DataLogger.Submit(serverLog);
        }
    }
}