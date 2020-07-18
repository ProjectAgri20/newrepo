using System;
using System.DirectoryServices.AccountManagement;
using System.Windows.Forms;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest.UI.Framework
{
    /// <summary>
    /// Dialog for collecting and authenticating user credentials.
    /// </summary>
    public partial class MainFormLogOnDialog : Form
    {
        private ErrorProvider _errorProvider = new ErrorProvider();

        /// <summary>
        /// Form constructor.  Initialize components and configure display style.
        /// </summary>
        public MainFormLogOnDialog()
        {
            InitializeComponent();
            InitializeErrorProvider();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
        }

        /// <summary>
        /// Form constructor.  Initialize components and configure display style.
        /// </summary>
        public MainFormLogOnDialog(bool validateOnly)
            : this()
        {
            connection_Label.Visible = !validateOnly;
            guest_CheckBox.Visible = !validateOnly;
        }

        /// <summary>
        /// Gets or sets the entered user name.
        /// </summary>
        public string UserName
        {
            get { return username_TextBox.Text; }
            set { username_TextBox.Text = value; }
        }

        /// <summary>
        /// Gets the entered password.
        /// </summary>
        public string Password => password_TextBox.Text;

        /// <summary>
        /// Gets the selected domain.
        /// </summary>
        public string Domain => domain_ComboBox.Text;

        /// <summary>
        /// Form OnLoad event handler.  Sets last login.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Load domain list
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                domain_ComboBox.DataSource = User.SelectDistinctDomain(context);
            }

            connection_Label.Text = "Connecting to:   {0} Environment".FormatWith(GlobalSettings.Items[Setting.Environment]);
        }

        /// <summary>
        /// Form OnActivated event handler.  Set focus to the correct control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            SetFocus();
        }

        private void InitializeErrorProvider()
        {
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            _errorProvider.ContainerControl = this;
            _errorProvider.SetIconAlignment(domain_ComboBox, ErrorIconAlignment.MiddleLeft);
            _errorProvider.SetIconAlignment(username_TextBox, ErrorIconAlignment.MiddleLeft);
            _errorProvider.SetIconAlignment(password_TextBox, ErrorIconAlignment.MiddleLeft);

        }

        /// <summary>
        /// Validation for Textboxes.  Checks for entered value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_HasValue(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Control textBox = (Control)sender;
            bool missingValue = string.IsNullOrWhiteSpace(textBox.Text) && !guest_CheckBox.Checked;

            _errorProvider.SetError(textBox, missingValue ? "Missing value" : string.Empty);
            e.Cancel = missingValue;
        }

        /// <summary>
        /// Determine which control should recieve the focus.
        /// </summary>
        private void SetFocus()
        {
            if (domain_ComboBox.SelectedIndex == -1)
            {
                domain_ComboBox.Focus();
            }
            else if (username_TextBox.Text.Length == 0)
            {
                username_TextBox.Focus();
            }
            else
            {
                password_TextBox.SelectAll();
                password_TextBox.Focus();
            }
        }

        /// <summary>
        /// Login button click event.  If the user selected "Login as Guest", sets the form DialogResult to Ignore.
        /// Otherwise, authenticates the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logIn_Button_Click(object sender, EventArgs e)
        {
            if (guest_CheckBox.Checked)
            {
                this.DialogResult = DialogResult.Ignore;
            }
            else
            {
                Authenticate();
            }
        }

        /// <summary>
        /// Authenticates the entered user credentials against the domain.
        /// </summary>
        private void Authenticate()
        {
            string message = string.Empty;

            if (this.ValidateChildren(ValidationConstraints.Enabled))
            {
                // Authenticate with the domain
                message = Authenticate(username_TextBox.Text, password_TextBox.Text, domain_ComboBox.Text);
                if (message.Length > 0) //Authentication failed
                {
                    MessageBox.Show(message, "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    SetFocus();
                }
                else
                {
                    // Close the form
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        private static string Authenticate(string userName, string password, string domain)
        {
            try
            {
                TraceFactory.Logger.Debug("User: {0}  Domain: {1}".FormatWith(userName, domain));
                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, domain))
                {
                    if (!context.ValidateCredentials(userName, password, ContextOptions.Negotiate | ContextOptions.Signing | ContextOptions.Sealing))
                    {
                        return "Invalid username or password.";
                    }
                }
            }
            catch (PrincipalException ex)
            {
                TraceFactory.Logger.Error(ex);
                return ex.Message;
            }

            return string.Empty;
        }

        private void guest_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            domain_ComboBox.Enabled = !guest_CheckBox.Checked;
            username_TextBox.Enabled = !guest_CheckBox.Checked;
            password_TextBox.Enabled = !guest_CheckBox.Checked;
        }
    }
}
