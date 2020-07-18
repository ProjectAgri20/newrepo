using System;
using System.Windows.Forms;
using HP.ScalableTest.UI;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;

namespace HP.ScalableTest
{
    /// <summary>
    /// A simple dialog for gathering printer connection information.
    /// </summary>
    public partial class SelectPrinterDialog : Form
    {
        /// <summary>
        /// Gets the primary address entered.
        /// </summary>
        public string Address1
        {
            get { return textBox_Address1.Text; }
        }

        /// <summary>
        /// Gets the manual entry setting.
        /// </summary>
        public bool ManualEntry
        {
            get { return manualCheckBox.Checked; }
        }

        /// <summary>
        /// Gets the device password entered.
        /// </summary>
        public string DevicePassword
        {
            get { return textBox_Password.Text; }
        }

        /// <summary>
        /// Gets the secondary address entered.
        /// </summary>
        public string Address2
        {
            get { return textBox_Address2.Text; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectPrinterDialog"/> class.
        /// </summary>
        public SelectPrinterDialog()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);
            bool visible = SecondaryAddressVisible();
            label_Address2.Visible = visible;
            textBox_Address2.Visible = visible;
        }

        private void manualCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (manualCheckBox.Checked)
            {
                textBox_Address1.Text = string.Empty;
                textBox_Address1.Enabled = false;

                textBox_Password.Text = string.Empty;
                textBox_Password.Enabled = false;
            }
            else
            {
                textBox_Address1.Enabled = true;
                textBox_Password.Enabled = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.BringToFront();
        }

        private bool SecondaryAddressVisible()
        {
            bool enterpriseEnabled = false;

            try
            {
                enterpriseEnabled = GlobalSettings.Items[Setting.EnterpriseEnabled].Equals("True", StringComparison.InvariantCultureIgnoreCase);
            }
            catch (SettingNotFoundException)
            {
            }

            return GlobalSettings.IsDistributedSystem || enterpriseEnabled;
        }
    }
}
