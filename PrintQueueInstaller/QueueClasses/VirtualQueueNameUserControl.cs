using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.UI;

namespace HP.ScalableTest.Print.Utility
{
    public partial class VirtualQueueNameUserControl : FieldValidatedControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualQueueNameUserControl"/> class.
        /// </summary>
        public VirtualQueueNameUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the virtual printer address.
        /// </summary>
        public string VirtualPrinterAddress
        {
            get { return virtualPrinterServerAddress_ComboBox.Text; }
        }

        /// <summary>
        /// Gets the number of queues.
        /// </summary>
        public int NumberOfQueues
        {
            get 
            {
                int result;
                if (int.TryParse(numberOfQueues_TextBox.Text, out result))
                {
                    return result;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the starting value for the forth octect of the IP address.
        /// </summary>
        public int StartingIPValue
        {
            get
            {
                int result;
                if (int.TryParse(startValue_TextBox.Text, out result))
                {
                    return result;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets the ending value for the forth octect of the IP address.
        /// </summary>
        public int EndingIPValue
        {
            get
            {
                int result;
                if (int.TryParse(endValue_TextBox.Text, out result))
                {
                    return result;
                }
                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether to increment port number.
        /// </summary>
        /// <value>
        ///   <c>true</c> if increment port number; otherwise, <c>false</c>.
        /// </value>
        public bool IncrementIPValue
        {
            get { return incrementIP_CheckBox.Checked; }
        }

        /// <summary>
        /// Gets a value indicating whether to share queues.
        /// </summary>
        /// <value>
        ///   <c>true</c> if share queues; otherwise, <c>false</c>.
        /// </value>
        public bool ShareQueues
        {
            get { return shared_CheckBox.Checked; }
        }

        /// <summary>
        /// Gets a value indicating whether render on client.
        /// </summary>
        /// <value>
        ///   <c>true</c> if render on client; otherwise, <c>false</c>.
        /// </value>
        public bool RenderOnClient
        {
            get { return renderClient_CheckBox.Checked; }
        }

        /// <summary>
        /// Gets a value indicating whether to enable SNMP.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enable SNMP; otherwise, <c>false</c>.
        /// </value>
        public bool EnableSnmp
        {
            get { return snmp_CheckBox.Checked; }
        }

        /// <summary>
        /// Gets the address code.
        /// </summary>
        public string AddressCode
        {
            get { return hostnameCode_TextBox.Text; }
        }

        /// <summary>
        /// Sets the focus.
        /// </summary>
        public void SetFocus()
        {
            virtualPrinterServerAddress_ComboBox.Focus();
        }

        private void ok_Button_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                ((Form)Parent).DialogResult = DialogResult.OK;
            }

        }

        private void numberOfQueues_TextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidatePositiveInt(numberOfQueues_TextBox.Text, "Number of Queues", numberOfQueues_TextBox, e);
        }

        private void startValue_TextBox_Validating(object sender, CancelEventArgs e)
        {
            string errorMessage = null;
            int val = this.StartingIPValue;

            if (val < 1 || val > 255)
            {
                errorMessage = "IP Start Value must be between 1 and 255.";
            }

            fieldValidator.SetError(startValue_TextBox, errorMessage);
            e.Cancel = errorMessage != null;
        }

        private void virtualPrinterServerAddress_TextBox_Validating(object sender, CancelEventArgs e)
        {
            HasValue(virtualPrinterServerAddress_ComboBox.Text, "Virtual Printer Server", virtualPrinterServerAddress_ComboBox, e);
        }

        private void cancel_Button_Click(object sender, EventArgs e)
        {
            ((Form)Parent).DialogResult = DialogResult.Cancel;
            ((Form)Parent).Close();
        }

        private void VirtualQueueDataControl_Load(object sender, EventArgs e)
        {
            // This will prevent the message textbox from being highlighted
            TabStop = false;

            if (this.DesignMode)
            {
                return;
            }

            // Load the virtual print servers from the Server Inventory table
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                var servers = context.FrameworkServers.Where(n => n.ServerTypes.Any(m => m.Name == "VPrint") && n.Active).Select(n => n.HostName).ToList();
                virtualPrinterServerAddress_ComboBox.DataSource = servers;
            }
        }

        private void virtualPrinterServerAddress_ComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string serverCode = string.Empty;
            string server = virtualPrinterServerAddress_ComboBox.Text;

            Match match = Regex.Match(server, "^STFVPRINT([0-9]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                serverCode = "VP{0}".FormatWith(match.Groups[1].Value);
            }
            else
            {
                serverCode = "VP";
            }

            hostnameCode_TextBox.Text = serverCode;
        }

        private void numberOfQueues_TextBox_TextChanged(object sender, EventArgs e)
        {
            int numberOfQueues = this.NumberOfQueues;

            if (numberOfQueues > 0)
            {
                int startValue = this.StartingIPValue;
                startValue_TextBox.Text = startValue.ToString(CultureInfo.InvariantCulture);

                int endValue = (numberOfQueues <= 255) ? numberOfQueues + startValue - 1 : 255;
                endValue_TextBox.Text = endValue.ToString(CultureInfo.InvariantCulture);
            }

        }

        private void startValue_TextBox_TextChanged(object sender, EventArgs e)
        {
            int startValue = this.StartingIPValue;

            if (startValue > 0)
            {
                int numberOfQueues = this.NumberOfQueues;
                numberOfQueues_TextBox.Text = numberOfQueues.ToString(CultureInfo.InvariantCulture);

                int endValue = this.EndingIPValue;
                endValue_TextBox.Text = endValue.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void NumbersOnlyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Determine whether the keystroke is a number from either the numbers on the keyboard or the keypad
            e.SuppressKeyPress = ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) && (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && e.KeyCode != Keys.Back);

        }
    }
}
