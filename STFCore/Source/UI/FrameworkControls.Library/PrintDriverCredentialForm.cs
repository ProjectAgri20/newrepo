using System;
using System.Security;
using System.Windows.Forms;
using System.ComponentModel;
using HP.ScalableTest.Data.EnterpriseTest;
using System.Net;
using HP.ScalableTest.Framework;
using System.IO;
using HP.ScalableTest.WindowsAutomation;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.UI.Framework
{

    public partial class PrintDriverCredentialForm : Form
    {
        /// <summary>
        /// Gets or sets the share location.
        /// </summary>
        public string ShareLocation
        {
            get
            {
                return shareLocation_TextBox.Text;
            }
            set
            {
                shareLocation_TextBox.Text = value;
            }
        }

        public PrintDriverCredentialForm()
        {
            InitializeComponent();
            if (UserManager.UserLoggedIn)
            {
                NetworkCredential credential = UserManager.CurrentUser.ToNetworkCredential();
                domain_TextBox.Text = credential.Domain;
                userName_TextBox.Text = credential.UserName;
            }
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (ValidateCredentials())
            {
                DialogResult = DialogResult.OK;
                Close();                
            }            
        }

        private bool ValidateCredentials()
        {
            bool successful = false;
            try
            {
                string shareName = Path.GetPathRoot(ShareLocation);

                NetworkCredential credential = new NetworkCredential(userName_TextBox.Text, password_TextBox.Text, domain_TextBox.Text);

                // Attempt to connect to share
                NetworkConnection.AddConnection(shareName, credential);
                successful = true;
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show(this, "Failed to connect to {0}: {1}".FormatWith(ShareLocation, ex.Message), this.Text);
            }

            return successful;
        }
    }
}
