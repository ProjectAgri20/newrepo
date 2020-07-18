using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace EmailCleaner
{
    public partial class MainForm : Form
    {
        string _dnsDomain = "bogus";

        public MainForm()
        {
            InitializeComponent();
            GlobalSettings.Load("stfsystem03.etl.boi.rd.adapps.hp.com");

            _dnsDomain = GlobalSettings.Items[Setting.DnsDomain];

        }

        private string UserAccount
        {
            get { return $"{maskedTextBox1.Text}@{_dnsDomain}"; }
        }

        private void SetStatus(string message)
        {
            this.InvokeIfRequired(c =>
                {
                    statusLabel.Text = message;
                }
                );
        }

        private void buttonClean_Click(object sender, EventArgs e)
        {
            try
            {
                buttonClean.Enabled = false;
                string message = $"Warning: This will delete all email from all folders for user account {UserAccount}\nClick OK to continue";
                var dr = MessageBox.Show(message, "Clear Email", MessageBoxButtons.OKCancel);
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    ToggleFormState(true);
                    Task.Factory.StartNew(() =>
                        {
                            SetStatus($"Connecting to Exchange account {UserAccount}...");
                            var emailController = ConfigureEmailController();
                            SetStatus($"Cleaning account {UserAccount}...");
                            foreach (EmailFolder folder in EnumUtil.GetValues<EmailFolder>())
                            {
                                emailController.Clear(folder);
                            }
                            SetStatus($"Finished cleaning {UserAccount}");
                            MessageBox.Show($"Email cleanup complete for {UserAccount}");
                        }
                    )
                    .ContinueWith(_ =>
                        {
                            ToggleFormState(false);
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:\n{ex.ToString()}");
                ToggleFormState(false);
            }
        }

        private void ToggleFormState(bool setAsRunning)
        {
            buttonClean.InvokeIfRequired(c =>
                {
                    if (setAsRunning)
                    {
                        this.UseWaitCursor = true;
                        buttonClean.Enabled = false;
                        Application.DoEvents();
                    }
                    else
                    {
                        this.UseWaitCursor = false;
                        buttonClean.Enabled = true;
                        Application.DoEvents();
                    }
                }
            );
        }

        private IEmailController ConfigureEmailController()
        {
            string userName = UserAccount;
            MailAddress userAddress = new MailAddress(UserAccount);
            string password = GlobalSettings.Items[Setting.OfficeWorkerPassword];
            string domain = GlobalSettings.Items[Setting.Domain];
            var credential = new NetworkCredential(userName, password, domain);
            IEmailController result = new ExchangeEmailController(credential, userAddress);
            return result;
        }
    }
}
