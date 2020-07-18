using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Framework.Properties;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// A message box for displaying exception information.
    /// </summary>
    public partial class ExceptionDialog : Form
    {
        private int _lastBottomPanelHeight = 0;

        /// <summary>
        /// Occurs when the Submit Error button is clicked.
        /// </summary>
        public event EventHandler<ExceptionDetailEventArgs> SubmitError;

        #region Properties

        /// <summary>
        /// Gets or sets the exception message.
        /// </summary>
        /// <value>
        /// The exception message.
        /// </value>
        public string ExceptionMessage
        {
            get { return exceptionMessage_TextBox.Text; }
            set { exceptionMessage_TextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the exception detail.
        /// </summary>
        /// <value>
        /// The exception detail.
        /// </value>
        public string ExceptionDetail
        {
            get { return exceptionDetail_TextBox.Text; }
            set { exceptionDetail_TextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>
        /// The user name.
        /// </value>
        public string UserName
        {
            get { return userName_TextBox.Text; }
            set { userName_TextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string HostName
        {
            get { return hostName_TextBox.Text; }
            set { hostName_TextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the name of the user logged into the host machine.
        /// </summary>
        /// <value>
        /// The host user.
        /// </value>
        public string HostUser
        {
            get { return hostUser_TextBox.Text; }
            set { hostUser_TextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address
        {
            get { return address_TextBox.Text; }
            set { address_TextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the assembly name.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public string AssemblyName
        {
            get { return assembly_TextBox.Text; }
            set { assembly_TextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the assembly version.
        /// </summary>
        /// <value>
        /// The assembly version.
        /// </value>
        public string AssemblyVersion
        {
            get { return version_TextBox.Text; }
            set { version_TextBox.Text = value; }
        }

        public DateTime Timestamp
        {
            get { return DateTime.Parse(timestamp_TextBox.Text); }
            set { timestamp_TextBox.Text = value.ToString("G"); }
        }

        /// <summary>
        /// Gets or sets the user notes.
        /// </summary>
        /// <value>
        /// The user notes.
        /// </value>
        public string UserNotes
        {
            get { return userNotes_TextBox.Text; }
            set { userNotes_TextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        /// <value>
        /// The user email.
        /// </value>
        public string UserEmail
        {
            get { return userEmail_TextBox.Text; }
            set { userEmail_TextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the submit was successful.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the submit was successful; otherwise, <c>false</c>.
        /// </value>
        public bool SubmitSuccessful
        {
            get { return submitted_ToolStripButton.Visible; }
            set
            {
                submit_ToolStripButton.Visible = !value;
                submitted_ToolStripButton.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can choose to
        /// ignore the exception and try to continue.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the user can continue; otherwise, <c>false</c>.
        /// </value>
        public bool AllowContinue
        {
            get { return continue_Button.Visible; }
            set
            {
                continue_Button.Visible = value;
                instructions_TextBox.Text = value ?
                    Resources.ExceptionInstructionsContinueQuit :
                    Resources.ExceptionInstructionsQuitOnly;
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDialog"/> class.
        /// </summary>
        public ExceptionDialog()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.SizeableDialog);

            // Populate environment information
            UserName = HostUser = Environment.UserName;
            HostName = Dns.GetHostName();
            Address = Dns.GetHostEntry(HostName).AddressList
                .First(n => n.AddressFamily == AddressFamily.InterNetwork).ToString();
            AssemblyName = Assembly.GetEntryAssembly().GetName().Name;
            AssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDialog"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="detail">The detail.</param>
        public ExceptionDialog(string message, string detail)
            : this()
        {
            ExceptionMessage = message;
            ExceptionDetail = detail;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDialog"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public ExceptionDialog(Exception ex)
            : this()
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            ExceptionMessage = ex.JoinAllErrorMessages();
            ExceptionDetail = ex.ToString();
        }

        /// <summary>
        /// On Load event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ToggleFormSize();
        }

        /// <summary>
        /// Handles the Click event of the exceptionDetails_Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void exceptionDetails_Button_Click(object sender, EventArgs e)
        {
            ToggleFormSize();
        }

        private void ToggleFormSize()
        {
            if (tabControl.Visible)
            {
                if (bottom_Panel.Height > 0)
                {
                    _lastBottomPanelHeight = bottom_Panel.Height;
                }

                bottom_Panel.Visible = false;
                exceptionDetails_Button.Image = expandCollapse_ImageList.Images["Expand"];
                this.Size = new System.Drawing.Size(this.Width, top_Panel.Height + 45);
            }
            else
            {
                bottom_Panel.Visible = true;
                exceptionDetails_Button.Image = expandCollapse_ImageList.Images["Collapse"];
                this.Size = new System.Drawing.Size(this.Width, top_Panel.Height + _lastBottomPanelHeight + 45);
            }
        }

        /// <summary>
        /// Handles the Click event of the submit_ToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void submit_ToolStripButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            TraceFactory.Logger.Fatal(ExceptionDetail);
            if (SubmitError != null)
            {
                SubmitError(this, new ExceptionDetailEventArgs(ExceptionMessage, ExceptionDetail));
            }

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handles the Click event of the submitted_ToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void submitted_ToolStripButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you wish to submit this error again?",
                                      "Confirm Resubmission",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Warning,
                                      MessageBoxDefaultButton.Button2,
                                      (MessageBoxOptions)0);

            if (result == DialogResult.Yes)
            {
                submit_ToolStripButton_Click(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of the save_ToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void save_ToolStripButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog.FileName, ExceptionDetail);
            }
        }

        /// <summary>
        /// Handles the Click event of the copy_ToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void copy_ToolStripButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(ExceptionDetail);
        }

        private void ExceptionDialog_Resize(object sender, EventArgs e)
        {

        }
    }
}
