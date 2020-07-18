using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.UI.SessionExecution
{
    public partial class InsufficientPlatformsErrorForm : Form
    {
        private SortableBindingList<ScenarioPlatformUsage> _platformUsage = null;
        private readonly string _scenarioName = string.Empty;

        public InsufficientPlatformsErrorForm(ScenarioPlatformUsageSet usages)
        {
            InitializeComponent();
            _scenarioName = usages.Scenario.Name;
            _platformUsage = new SortableBindingList<ScenarioPlatformUsage>();
            foreach (var usage in usages)
            {
                _platformUsage.Add(usage);
            }
        }

        private void MachineQuotaForm_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                scenario_TextBox.Text = _scenarioName;

                quota_DataGridView.DataSource = _platformUsage;

                foreach (DataGridViewRow row in quota_DataGridView.Rows)
                {
                    var data = row.DataBoundItem as ScenarioPlatformUsage;

                    if (data.AuthorizedCount < data.RequiredCount || (data.AuthorizedCount == 0 && data.RequiredCount == 0))
                    {
                        row.Cells[0].Value = platform_ImageList.Images["Cross"];
                    }
                    else
                    {
                        row.Cells[0].Value = platform_ImageList.Images["Tick"];
                    }
                }

                submit_LinkLabel.Visible = _platformUsage.Any(x => x.RequiredCount > x.AuthorizedCount);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void quota_DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            quota_DataGridView.ClearSelection();
        }

        private void submit_LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string server = GlobalSettings.Items[Setting.AdminEmailServer];
            string addresses = GlobalSettings.Items[Setting.AdminEmailAddress];

            if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(addresses))
            {
                UnableToSendMessage();
                return;
            }

            try
            {
                using (SmtpClient client = new SmtpClient(server))
                {
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress("donotreply@hp.com", "STF Automation");

                        foreach (string address in addresses.Split(';'))
                        {
                            message.To.Add(new MailAddress(address));
                            message.ReplyToList.Add(address);
                        }

                        message.Subject = "{0} - Machine Quota Increase".FormatWith(UserManager.CurrentUserName);
                        string body = Properties.Resources.MachineAssignmentIncrease.Replace("{USER}", UserManager.CurrentUserName);
                        body = body.Replace("{SCENARIO}", _scenarioName);

                        StringBuilder builder = new StringBuilder();
                        foreach (var usage in _platformUsage)
                        {
                            builder.AppendLine("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>".FormatWith(usage.PlatformId, usage.RequiredCount, usage.AuthorizedCount));
                        }

                        message.IsBodyHtml = true;
                        message.Body = body.Replace("{ROWS}", builder.ToString());

                        client.Send(message);

                        MessageBox.Show("A request for more platforms has been sent to {0}".FormatWith(string.Join(", ", addresses.Split(';'))));
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                TraceFactory.Logger.Debug(ex.Message);
                UnableToSendMessage();
            }
            catch (SmtpException ex)
            {
                TraceFactory.Logger.Error(ex.Message);
                UnableToSendMessage();
            }
        }

        private void UnableToSendMessage()
        {
            MessageBox.Show("Unable to send email to administrator, please contact directly", "Unable to Send Email", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
