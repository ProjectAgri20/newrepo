using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.PluginSupport.Connectivity.UI
{
    /// <summary>
    /// Control for providing details about switch such as the switch IP Address, switch port number.
    /// </summary>
    public partial class SwitchDetailsControl : UserControl
    {
        /// <summary>
        /// Occurs when a property value is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _hidePortNumber = false;

        /// <summary>
        /// Create an instance of the SwitchDetailsControl.
        /// </summary>
        public SwitchDetailsControl()
        {
            InitializeComponent();

            switch_FieldValidator.RequireCustom(switchIPAddress_IpAddressControl, ValidateIPAddress, "Enter valid IP address");

            switch_FieldValidator.SetIconAlignment(switchIPAddress_IpAddressControl, ErrorIconAlignment.MiddleRight);
            switchPortNumber_NumericUpDown.Enabled = !_hidePortNumber;
            portNumber_Label.Enabled = !_hidePortNumber;
        }

        /// <summary>
		/// Validating the control elements
		/// </summary>
        /// <returns></returns>
		public ValidationResult ValidateControls()
        {
            bool status = switch_FieldValidator.ValidateAll().Aggregate(true, (current, result) => current & result.Succeeded);

            return new ValidationResult(status);
        }

        /// <summary>
        /// Validates IP address
        /// </summary>
        /// <returns></returns>
        private bool ValidateIPAddress()
        {
            return switchIPAddress_IpAddressControl.IsValidIPAddress();
        }

        /// <summary>
        /// Gets or sets the port Number
        /// </summary>
        public int SwitchPortNumber
        {
            get
            {
                return Convert.ToInt32(switchPortNumber_NumericUpDown.Value);
            }
            set
            {
                if (value < switchPortNumber_NumericUpDown.Minimum)
                {
                    switchPortNumber_NumericUpDown.Value = switchPortNumber_NumericUpDown.Minimum;
                }
                else if (value > switchPortNumber_NumericUpDown.Maximum)
                {
                    switchPortNumber_NumericUpDown.Value = switchPortNumber_NumericUpDown.Maximum;
                }
                else
                {
                    switchPortNumber_NumericUpDown.Value = Convert.ToInt32(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the IP Address of the switch
        /// </summary>
        public string SwitchIPAddress
        {
            get
            {
                return !string.IsNullOrEmpty(switchIPAddress_IpAddressControl.Text) ? switchIPAddress_IpAddressControl.Text : string.Empty;
            }
            set
            {
                switchIPAddress_IpAddressControl.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets value indicating whether validation is required on the control
        /// </summary>
        public bool ValidationRequired { get; set; }

        /// <summary>
        /// Occurs on property changed
        /// </summary>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs on value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"><see cref="EventArgs"/></param>
        private void switchPortNumber_NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(SwitchPortNumber.ToString()));
        }

        /// <summary>
        /// Occurs on text changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"><see cref="EventArgs"/></param>
        private void switchIPAddress_IpAddressControl_TextChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(SwitchIPAddress));
        }

        [Description("Flag to hide the execute on interface"), Category("Data")]
        public bool HidePortNumber
        {
            get { return _hidePortNumber; }
            set
            {
                _hidePortNumber = value;
                switchPortNumber_NumericUpDown.Enabled = !_hidePortNumber;
                portNumber_Label.Enabled = !_hidePortNumber;
            }
        }
    }
}