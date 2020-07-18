using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Email
{
    /// <summary>
    /// Inbox control for the Email activity control.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ExchangeInboxControl : UserControl
    {
        /// <summary>
        /// Indicates if the inbox is currently loading new messages.
        /// </summary>
        private bool _loadingInbox = false;
        private object _receiveEmailLock = new object();
        private IEmailController _emailController;

        /// <summary>
        /// Constructs the ExchangeInboxControl control.
        /// </summary>
        public ExchangeInboxControl(IEmailController controller)
        {
            InitializeComponent();
            _emailController = controller;
            inboxGridView.AutoGenerateColumns = false;
        }

        /// <summary>
        /// Configure the object prior to the ExchangeInboxControl control being loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExchangeInboxControl_Load(object sender, EventArgs e)
        {
            _emailController.StatusUpdate += EmailController_EmailReceived;
        }

        /// <summary>
        /// Kicks off the process of getting the inbox messages, then updating the GUI with what was found.
        /// </summary>
        /// <remarks>
        /// This method does this loading in the background, thus calling this method does not guarantee that
        /// the Inbox will be loaded.
        /// </remarks>
        public void LoadInbox()
        {
            if (_loadingInbox)
                return;

            _loadingInbox = true;

            // Kick off the Inbox refresh in the background
            ThreadPool.QueueUserWorkItem(worker_DoWork);
        }

        /// <summary>
        /// Performs the Email Send Activity.
        /// It is important to note that this method will be executing in a background thread.
        /// </summary>
        /// <param name="notUsed">Not used.</param>
        private void worker_DoWork(object notUsed)
        {
            Thread.CurrentThread.SetName("ReceiveEmailActivity");
            lock (_receiveEmailLock)
            {
                try
                {
                    Collection<EmailMessage> messages = GetInboxItems();
                    if (inboxGridView.InvokeRequired)
                    {
                        inboxGridView.Invoke(new MethodInvoker(() => UpdateGrid(messages)));
                    }
                    else
                    {
                        UpdateGrid(messages);
                    }

                }
                finally
                {
                    _loadingInbox = false;
                }
            }
        }
        /// <summary>
        /// Get the Inbox messages.
        /// </summary>
        /// <returns>List of Email messages from the Inbox.</returns>
        private Collection<EmailMessage> GetInboxItems()
        {
            try
            {
                Collection<EmailMessage> messages = _emailController.RetrieveMessages(EmailFolder.Inbox);
                // Take this opportunity to clean out the inbox.
                // This is being done by the EmailMonitor
                //_emailController.Delete(messages);
                //_emailController.Clean(EmailFolder.SentItems);
                return messages;
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Updates the Inbox grid.  This method is responsible
        /// for taking the Email messages and putting them into the GUI
        /// </summary>
        /// <param name="items">Email messages to post</param>
        private void UpdateGrid(Collection<EmailMessage> items)
        {
            UpdatePrompt("Refreshing Inbox...");

            inboxGridView.DataSource = null;
            inboxGridView.DataSource = items;
            inboxGridView.Refresh();

            // Inbox has been updated.  Remove the prompt.
            UpdatePrompt(string.Empty);
        }

        private void EmailController_EmailReceived(object sender, StatusChangedEventArgs e)
        {
            UpdatePrompt(e.StatusMessage);
        }

        /// <summary>
        /// Updates the prompt on the control.
        /// </summary>
        /// <param name="prompt"></param>
        private void UpdatePrompt(string prompt)
        {
            // This method must execute in the GUI thread.
            if (refreshLabel.InvokeRequired)
            {
                refreshLabel.Invoke(new MethodInvoker(() => UpdatePrompt(prompt)));
                return;
            }

            ExecutionServices.SystemTrace.LogDebug(prompt);
            refreshLabel.Text = prompt;
            refreshLabel.Refresh();
        }
    }
}
